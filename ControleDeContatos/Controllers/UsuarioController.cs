using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();
            return View(usuarios);
        }
        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);

            UsuarioSemSenhaModel usuarioSemSenha = new UsuarioSemSenhaModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Login = usuario.Login,
                Email = usuario.Email,
                Perfil = usuario.Perfil
            };

            return View(usuarioSemSenha);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }
        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _usuarioRepositorio.Apagar(id);

                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Usuario apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Não conseguimos apagar seu usuario!";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não foi possível apagar seu usuario, tente novamente, detalhe do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuario cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não foi possível cadastrar seu usuario, tente novamente, detalhe do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuariosemsenhamodel)
        {
            try
            {
                UsuarioModel usuario = null;

                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuariosemsenhamodel.Id,
                        Nome = usuariosemsenhamodel.Nome,
                        Login = usuariosemsenhamodel.Login,
                        Email = usuariosemsenhamodel.Email,
                        Perfil = usuariosemsenhamodel.Perfil
                    };

                    _usuarioRepositorio.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Usuario atualizado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View("Editar", usuariosemsenhamodel);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Não foi possível alterar seu usuario, tente novamente, detalhe do erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}