namespace APICatalogo.Services
{
    //Registrar o servoço no arquivo Program.cs 
    //builder.Services.AddTransient<IMeuServico, MeuServico>();
    public interface IMeuServico
    {
        string Saudacao(string nome);
    }
}
