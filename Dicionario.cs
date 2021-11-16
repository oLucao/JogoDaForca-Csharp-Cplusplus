//Lucas Abreu dos Santos 21245
//Talita de Almeida Barbosa 21261 --meu projeto
using System;
using System.IO;
using System.Windows.Forms;

class Dicionario
{
  Dicionario[] dados;  // vetor de Funcionario
  int qtosDados;        // tamanho lógico
  int posicaoAtual;

  const int inicioPalavra = 0,
            tamanhoPalavra = 15,
            inicioDica = inicioPalavra + tamanhoPalavra;
           

  string palavra;
  string dica;

  public Dicionario(string pal, string dica)
  {
    this.Palavra = pal;
    this.Dica = dica;
  }

  public Dicionario()
  {
    Palavra = " ";
    Dica    = " ";
  }

  public string Palavra
  {
    get => palavra;
    set
    {
      if (value.Length > tamanhoPalavra)
        value = value.Substring(15, tamanhoPalavra);
      palavra = value.PadRight(tamanhoPalavra, ' ');
    }
  }

  public string Dica 
  { 
    get => dica; 
    set 
    { 
      dica = value; 
    } 
  }

  public void LerDados(StreamReader arq)
  {
    if (!arq.EndOfStream)
    {
      String linha = arq.ReadLine();
      Palavra = linha.Substring(inicioPalavra, tamanhoPalavra);
      Dica = linha.Substring(inicioDica);
    }
  }

  public String FormatoDeArquivo() 
  {
    return Palavra.ToString().PadLeft(tamanhoPalavra, ' ') +
           Dica; 
  }

}

