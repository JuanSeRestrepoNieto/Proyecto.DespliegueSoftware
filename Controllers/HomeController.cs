namespace Controllers;

public class HomeController
{
  public string HelloWorld()
  {
    const int edad = 12;
    string mensaje = string.Empty;
    mensaje = edad > 15 ? "Eres un pre adoslescente" : "Eres un niño";
    return mensaje;
  }

  public string SaludoDos()
  {
    string mensaje = "Hola mundo Dos";
    return mensaje;
  }

  public string GoodbyeWorld()
  {
    return "¡Adiós, mundo!";
  }
}