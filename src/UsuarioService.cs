using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ReservaDeSalas
{
    public class UsuarioService
    {
        private const string FilePath = "usuarios.json";
        private List<Usuario> _usuarios;

        public UsuarioService()
        {
            _usuarios = CarregarUsuarios();
            if (!_usuarios.Any(u => u.Nivel == NivelAcesso.Admin))
            {
                var admin = new Usuario("admin", "admin123") { Nivel = NivelAcesso.Admin, IsDocente = true }; // Admin também é docente para ter acesso total
                _usuarios.Add(admin);
                SalvarUsuarios();
            }
        }

        public Usuario? Autenticar(string nome, string senha)
        {
            return _usuarios.FirstOrDefault(u => u.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase) && u.Senha == senha);
        }

        public bool Registrar(string nome, string senha, NivelAcesso nivel)
        {
            if (_usuarios.Any(u => u.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)))
                return false;

            var novoUsuario = new Usuario(nome, senha) { 
                Nivel = nivel,
                IsDocente = (nivel == NivelAcesso.Docente || nivel == NivelAcesso.Admin) // Admin também é docente para fins de acesso
            };
            _usuarios.Add(novoUsuario);
            SalvarUsuarios();
            return true;
        }

        private List<Usuario> CarregarUsuarios()
        {
            if (!File.Exists(FilePath)) return new List<Usuario>();
            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
            }
            catch { return new List<Usuario>(); }
        }

        private void SalvarUsuarios()
        {
            string json = JsonSerializer.Serialize(_usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public List<Usuario> ListarTodos() => _usuarios;
    }
}
