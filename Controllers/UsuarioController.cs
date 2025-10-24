using Google.Cloud.Firestore;
using isatec_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace isatec_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly FirestoreDb _firestoreDb;

    public UsuarioController()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-key.json");
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
        _firestoreDb = FirestoreDb.Create("isatec-1"); // <-- substitui pelo ID do teu projeto Firebase
    }

    // GET: api/usuario
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var usuariosRef = _firestoreDb.Collection("usuarios");
        var snapshot = await usuariosRef.GetSnapshotAsync();

        var usuarios = snapshot.Documents.Select(doc => doc.ConvertTo<Usuario>()).ToList();
        return Ok(usuarios);
    }

    // POST: api/usuario
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Usuario novoUsuario)
    {
        var usuariosRef = _firestoreDb.Collection("usuarios");
        await usuariosRef.AddAsync(novoUsuario);
        return Ok(new { mensagem = "Usuário criado com sucesso!" });
    }

    // PUT: api/usuario/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Usuario usuario)
    {
        var docRef = _firestoreDb.Collection("usuarios").Document(id);
        await docRef.SetAsync(usuario, SetOptions.Overwrite);
        return Ok(new { mensagem = "Usuário atualizado com sucesso!" });
    }

    // DELETE: api/usuario/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var docRef = _firestoreDb.Collection("usuarios").Document(id);
        await docRef.DeleteAsync();
        return Ok(new { mensagem = "Usuário removido com sucesso!" });
    }
}
