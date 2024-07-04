using sell_systems.BLL.Interfaces;
using sell_systems.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sell_systems.DAL.Interfaces;
using System.Net;
using MySql.Data.MySqlClient;

namespace sell_systems.BLL.Implementacion
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IGenericRepository<Usuario> _repository;
        private readonly IFireBaseService _fireBaseService;
        private readonly IUtilidadesSerivce _utilidadesSerivce;
        private readonly ICorreoService _correoService;

        public UsuarioService(
                IGenericRepository<Usuario> repository,
                IFireBaseService fireBaseService,
                IUtilidadesSerivce utilidadesSerivce,
                ICorreoService correoService
            )
        {
            _repository = repository;
            _fireBaseService = fireBaseService;
            _utilidadesSerivce = utilidadesSerivce;
            _correoService = correoService;
        }
        public async Task<List<Usuario>> lista()
        {
            IQueryable<Usuario> query = await _repository.Consultar();
            return query.Include(r => r.IdRolNavigation).ToList();
        }

        public async Task<Usuario> crear(Usuario entidad, Stream foto = null, string nombreFoto = "", string urlPlantillaCorreo = "")
        {
            Usuario usuarioExiste = await _repository.Obtener(u=>u.Correo == entidad.Correo);
            if (usuarioExiste == null) {
                throw new TaskCanceledException("El usuario ya existe");
            }
            try
            {
                string claveGenerada = _utilidadesSerivce.GenerarClave();
                entidad.Clave = _utilidadesSerivce.ConvertirSha256(claveGenerada);
                entidad.NombreFoto = nombreFoto;
                if (foto != null) {
                    string urlFoto = await _fireBaseService.SubirStorage(foto, "carpeta_usuario", nombreFoto);
                    entidad.UrlFoto = urlFoto;
                }
                Usuario usuarioCreado = await _repository.Crear(entidad);

                if (usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario");

                if(urlPlantillaCorreo != "")
                    urlPlantillaCorreo = urlPlantillaCorreo.Replace("[correo]", usuarioCreado.Correo).Replace("[clave]", claveGenerada);
                
                string htmlCorreo = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPlantillaCorreo);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK ) {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader readerStream = null;
                        if(response.CharacterSet == null)
                            readerStream = new StreamReader(dataStream);
                        else
                            readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));

                        htmlCorreo = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();
                    }
                }
                if (htmlCorreo != "")
                    await _correoService.EnviarCorreo(usuarioCreado.Correo, "Cuenta Creada", htmlCorreo);   

                

                IQueryable<Usuario> query = await _repository.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
                usuarioCreado = query.Include(r => r.IdRolNavigation).First();

                return usuarioCreado;

            }
            catch (Exception ex) {
                throw;
            }

        }

        public async Task<Usuario> editar(Usuario entidad, Stream foto = null, string nombreFoto = "")
        {
            Usuario usuarioExiste = await _repository.Obtener(u => u.Correo == entidad.Correo && u.IdUsuario != entidad.IdUsuario);
            if (usuarioExiste == null)
                throw new TaskCanceledException("El usuario ya existe");

            try
            {
                IQueryable<Usuario> queryUsuario = await _repository.Consultar(u => u.IdUsuario == entidad.IdUsuario);
                Usuario usuarioEditar = queryUsuario.First();
                usuarioEditar.Nombre = entidad.Nombre;
                usuarioEditar.Correo= entidad.Correo;
                usuarioEditar.Telefono = entidad.Telefono;
                usuarioEditar.IdRol = entidad.IdRol;

                if (usuarioEditar.NombreFoto == "")
                    usuarioEditar.NombreFoto = entidad.NombreFoto;

                if (foto != null)
                {
                    string urlFoto = await _fireBaseService.SubirStorage(foto, "carpeta_usuario", usuarioEditar.NombreFoto);
                    usuarioEditar.NombreFoto = urlFoto;
                }

                bool respuesta = await _repository.Editar(usuarioEditar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo modificar el usuario");

                Usuario usuarioEditado = queryUsuario.Include(r => r.IdRolNavigation).First();

                return usuarioEditado;
            }
            catch {
                throw;
            }



        }

        public async Task<bool> eliminar(int idUsuario)
        {
            try
            {
                Usuario usuarioEncontrado = await _repository.Obtener(u => u.IdUsuario == idUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                string nombreFoto = usuarioEncontrado.NombreFoto;
                bool respuesta = await _repository.Eliminar(usuarioEncontrado);

                if (respuesta)
                    await _fireBaseService.EliminarStorage("carpeta_usuario", nombreFoto);

                return true;

            }
            catch {
                throw;
            }
        }



        public async Task<Usuario> obtenerPorCredenciales(string correo, string clave)
        {
            string claveEncriptada = _utilidadesSerivce.ConvertirSha256(clave);
            Usuario usuarioEncontrado = await _repository.Obtener(u => u.Correo.Equals(correo) && u.Clave.Equals(claveEncriptada));
            return usuarioEncontrado;

        }

        public async Task<Usuario> obtenerPorId(int idUsuario)
        {
            IQueryable<Usuario> query = await _repository.Consultar(u=>u.IdUsuario== idUsuario);
            Usuario resultado = query.Include(r=> r.IdRolNavigation).FirstOrDefault();

            return resultado;

        }
        public async Task<bool> guardarPerfil(Usuario entidad)
        {
            try
            {
                Usuario usuarioEncontrado = await _repository.Obtener(u=> u.IdUsuario == entidad.IdUsuario);

                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                usuarioEncontrado.Correo = entidad.Correo;
                usuarioEncontrado.Telefono = entidad.Telefono;

                bool respuesta = await _repository.Editar(usuarioEncontrado);
                return respuesta;

                
            }
            catch {
                throw;

            }
        }
        public async Task<bool> cambiarClave(int idUsuario, string claveActual, string claveNueva)
        {
            try
            {
                Usuario usuarioEncontrado = await _repository.Obtener(u => u.IdUsuario == idUsuario);
                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");
                if (usuarioEncontrado.Clave != _utilidadesSerivce.ConvertirSha256(claveActual))
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");
                usuarioEncontrado.Clave = _utilidadesSerivce.ConvertirSha256(claveNueva);
                bool respuesta = await _repository.Editar(usuarioEncontrado);

                return respuesta;
               
            }
            catch(Exception ex) 
            {
                throw;
            }
        }

        public async  Task<bool> restablecerClave(string correo, string urlPlantillaCorreo)
        {
            try
            {
                Usuario usuarioEncontrado = await _repository.Obtener(u => u.Correo == correo);
                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("No se encontro el usuario asociado al correo");
                string claveGenerada = _utilidadesSerivce.GenerarClave();
                usuarioEncontrado.Clave = _utilidadesSerivce.ConvertirSha256(claveGenerada);

                if (urlPlantillaCorreo != "")
                    urlPlantillaCorreo = urlPlantillaCorreo.Replace("[clave]", claveGenerada);

                string htmlCorreo = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPlantillaCorreo);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader readerStream = null;
                        if (response.CharacterSet == null)
                            readerStream = new StreamReader(dataStream);
                        else
                            readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));

                        htmlCorreo = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();
                    }
                }

                bool correoEnviado = false;

                if (htmlCorreo != "")
                    await _correoService.EnviarCorreo(correo, "Contraseña Restablecida", htmlCorreo);

                if (!correoEnviado)
                    throw new TaskCanceledException("Tenemos problemas por favor intentalo de nuevo mas tarde");

                bool respuesta = await _repository.Editar(usuarioEncontrado);
                return respuesta;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
