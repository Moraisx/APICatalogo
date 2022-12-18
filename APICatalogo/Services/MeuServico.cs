namespace APICatalogo.Services
{
    //Registrar o servoço no arquivo Program.cs 
    //builder.Services.AddTransient<IMeuServico, MeuServico>();
    public class MeuServico : IMeuServico
    {
        public string Saudacao(string nome)
        {
           return $"Bem vindo, {nome} \n\n {DateTime.Now}";
        }
    }
}
