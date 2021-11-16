//Lucas Abreu dos Santos 21245
//Talita de Almeida Barbosa 21261 
using System;
using System.IO;
using System.Windows.Forms;

namespace apVetorObjeto
{
  public partial class FrmForca : Form
  {
    VetorDicionario osDic;
    int posicaoDeInclusao;
    int num = 90;

    public FrmForca()
    {
      InitializeComponent();
    }

    private void btnSair_Click(object sender, EventArgs e)
    {
      Close();
    }
    Dicionario dadoLido = new Dicionario();
    Dicionario palavraSorteada = null;
    Button[] BotoesClicados = new Button[45];
    int botoesqtd = 0;

    private void FrmFunc_Load(object sender, EventArgs e)
    {
      int indice = 0;
      tsBotoes.ImageList = imlBotoes;
      foreach (ToolStripItem item in tsBotoes.Items)
        if (item is ToolStripButton) // se não é separador:
          (item as ToolStripButton).ImageIndex = indice++;

      osDic = new VetorDicionario(100); // instancia com vetor dados com 100 posições

      if (dlgAbrir.ShowDialog() == DialogResult.OK)
      {
        var arquivo = new StreamReader(dlgAbrir.FileName);
        while (!arquivo.EndOfStream)
        {
          var dadoLido = new Dicionario(); 
          dadoLido.LerDados(arquivo); // método da classe 
          osDic.Incluir(dadoLido);   // método de VetorFuncionario – inclui ao final
        }
        arquivo.Close();
        osDic.PosicionarNoPrimeiro(); // posiciona no 1o registro a visitação nos dados
        AtualizarTela();               // mostra na tela as informações do registro visitado agora 
      }
    }

    private void btnInicio_Click(object sender, EventArgs e)
    {
      osDic.PosicionarNoPrimeiro();
      AtualizarTela();
    }

    private void btnAnterior_Click(object sender, EventArgs e)
    {
      osDic.RetrocederPosicao();
      AtualizarTela();
    }

    private void AtualizarTela()
    {
      if (!osDic.EstaVazio)
      {
        int indice = osDic.PosicaoAtual;
        edPalavra.Text = osDic[indice].Palavra + "";
        edDica.Text = osDic[indice].Dica;
        stlbMensagem.Text = "Registro " + (osDic.PosicaoAtual + 1) +
        "/" + osDic.Tamanho;
        osDic.ExibirDados(dgvPalavraDica);
      }
    }

    private void TestarBotoes()
    {
      btnInicio.Enabled = true;
      btnVoltar.Enabled = true;
      btnAvancar.Enabled = true;
      btnUltimo.Enabled = true;
      if (osDic.EstaNoInicio)
      {
        btnInicio.Enabled = false;
        btnVoltar.Enabled = false;
      }

      if (osDic.EstaNoFim)
      {
        btnAvancar.Enabled = false;
        btnUltimo.Enabled = false;
      }
    }
    private void LimparTela()
    {
      edPalavra.Clear();
      edDica.Clear();
    }

    private void btnProximo_Click(object sender, EventArgs e)
    {
      osDic.AvancarPosicao();
      AtualizarTela();
    }

    private void btnUltimo_Click(object sender, EventArgs e)
    {
      osDic.PosicionarNoUltimo();
      AtualizarTela();
    }

    private void FrmFunc_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (dlgAbrir.FileName != "")  // foi selecionado um arquivo com dados
        osDic.GravarDados(dlgAbrir.FileName);
    }

    private void btnNovo_Click(object sender, EventArgs e)
    {
      // saímos do modo de navegação e entramos no modo de inclusão:
      osDic.SituacaoAtual = Situacao.incluindo;

      // preparamos a tela para que seja possível digitar dados da nova dica
      LimparTela();

      edPalavra.ReadOnly = false;
      // colocamos o cursor no campo chave
      edPalavra.Focus();
      lbMensagem.Text = "Digite uma nova dica para o jogo.";
    }

    private void edPalavra_Leave(object sender, EventArgs e)
    {
      if (osDic.SituacaoAtual == Situacao.incluindo ||
          osDic.SituacaoAtual == Situacao.pesquisando)
        if (edPalavra.Text == "")
        {
          MessageBox.Show("Digite uma palavra válida!");
          edPalavra.Focus();
        }
        else  // temos um valor digitado no edDica
        {
          string palavraProcurada = edPalavra.Text;
          int posicao;
          bool achouRegistro = osDic.Existe(palavraProcurada, out posicao);
          switch (osDic.SituacaoAtual)
          {
            case Situacao.incluindo:
              if (achouRegistro)
              {
                MessageBox.Show("Palavra repetida! Inclusão cancelada.");
                osDic.SituacaoAtual = Situacao.navegando;
                AtualizarTela(); // exibe novamente o registro que estava na tela antes de esta ser limpa
              }
              else  // a dica não existe e podemos incluí-la no índice ondeIncluir
              {     // incluí-la no índice ondeIncluir do vetor interno dados de osFunc
                edDica.Focus();
                lbMensagem.Text = "Digite os demais dados e pressione [Salvar].";
                btnSalvar.Enabled = true;  // habilita quando é possível incluir
                posicaoDeInclusao = posicao;  // guarda índice de inclusão em variável global
              }
              break;

            case Situacao.pesquisando:
              if (achouRegistro)
              {
                // a variável posicao contém o índice de dica que se buscou
                osDic.PosicaoAtual = posicao;   // reposiciona o índice da posição visitada
                AtualizarTela();
              }
              else
              {
                MessageBox.Show("Dica digitada não foi encontrada.");
                AtualizarTela();  // reexibir o registro que aparecia antes de limparmos a tela
              }

              osDic.SituacaoAtual = Situacao.navegando;
              edPalavra.ReadOnly = true;
              break;
          }
        }
    }

    private void btnSalvar_Click(object sender, EventArgs e)
    {
      if (osDic.SituacaoAtual == Situacao.incluindo)  // só guarda nova dica no vetor se estiver incluindo
      {
        // criamos objeto com o registro da nova dica digitado no formulário
        
        var novoDic = new Dicionario(edPalavra.Text,edDica.Text);

        osDic.Incluir(novoDic, posicaoDeInclusao);

        osDic.SituacaoAtual = Situacao.navegando;  // voltamos ao mode de navegação

        osDic.PosicaoAtual = posicaoDeInclusao;

        AtualizarTela();
      }
      else
        if (osDic.SituacaoAtual == Situacao.editando)
      {
        osDic[osDic.PosicaoAtual].Dica = edDica.Text;
        osDic.SituacaoAtual = Situacao.navegando;
      }
      btnSalvar.Enabled = false;    // desabilita pois a inclusão terminou
      edPalavra.ReadOnly = true;
    }

    private void btnExcluir_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Deseja realmente excluir?", "Exclusão",
          MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
      {
        osDic.Excluir(osDic.PosicaoAtual);
        if (osDic.PosicaoAtual >= osDic.Tamanho)
          osDic.PosicionarNoUltimo();
        AtualizarTela();
      }
    }

    private void btnProcurar_Click(object sender, EventArgs e)
    {
      osDic.SituacaoAtual = Situacao.pesquisando;  // entramos no modo de busca
      LimparTela();
      edDica.ReadOnly = false;
      edDica.Focus();
      MessageBox.Show("Digite a dica da palavra procurada.");
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
      osDic.SituacaoAtual = Situacao.navegando;
      AtualizarTela();
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
      // permitimos ao usuario editar o registro atualmente
      // exibido na tela
      osDic.SituacaoAtual = Situacao.editando;
      edDica.Focus();
      MessageBox.Show("Digite a nova dica da palavra e pressione [Salvar].");
      btnSalvar.Enabled = true;
      edDica.ReadOnly = true;  // não deixamos usuário alterar a palavra (chave primária)
    }

    private void tpLista_Enter(object sender, EventArgs e)
    {
      osDic.ExibirDados(dgvPalavraDica);
    }

    private void tmrTempo_Tick(object sender, EventArgs e)
    {

      if (num != -1)
      {
        lbTempo.Text = num + "s";//fazemos a contagem do tempo
        num--;
      }
      else
      {
        tmrTempo.Stop();
        MessageBox.Show("O tempo acabou!");//quando o tempo acabar mostramos uma mensagem
      }
    }

    private void cbArduino_CheckedChanged(object sender, EventArgs e)
    {
      if (cbArduino.Checked)
      {
        /* Muda o nome porta e  Abre a porta serial */
        sp.PortName = txtPortaSerial.Text;
        try
        {
          sp.Open();
        }
        catch (Exception)
        {
          MessageBox.Show("Erro ao abrir porta serial ...");
          cbArduino.Checked = false;
          return;
        }
      }
      else
      {
        sp.Close();  //fechar porta serial
      }
      HabilitaControles(cbArduino.Checked);
    }

    private void HabilitaControles(bool estado)
    {
      cbArduino.Enabled = estado;
    }
    private void btnIniciar_Click(object sender, EventArgs e)
    {
      {
        if (cbArduino.Checked)
        {
         str = "K";
         sp.Write(str);
        }
        tpCadastro.Enabled = false;
        string nome = txtNome.Text;//colocamos que nome recebe o que foi colocado textBox
        Random numAleatorio = new Random();
        var n = numAleatorio.Next(0, osDic.Tamanho - 1);
        palavraSorteada = osDic[n];

        if (cbComDica.Checked)
        {
          var dica = palavraSorteada.Dica;//Aqui se o cbComDica estiver clicado começamos a contagem do tempo e mostramos a dica
          lbDica.Text = dica;
          tmrTempo.Start();
        }
        dataGridView_Palavra.ColumnCount = palavraSorteada.Palavra.Trim().Length;
      }
    }

    private void sp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
    {
      // receberia os dados  e mostraria no textbox
    }
    int pontos = 0;
    int erros = 0;

    private void btnA_Click(object sender, EventArgs e)
    {
      char letra = (sender as Button).Text[0];
      (sender as Button).Enabled = false;//aqui relacionamos os botões com o click
      BotoesClicados[botoesqtd] = sender as Button;
      botoesqtd++;
      var posicao = palavraSorteada.Palavra.IndexOf(letra.ToString());
      if (posicao == -1)//aqui contaremos os erros
      {
        erros++;
        lbErros.Text = "" + erros;
        Erro();
        if( erros != 0 && cbArduino.Checked)//aqui se essa condição estar true começamos a fazer o método do projeto do professor Sérgio
        {
          Habilita();
        }
      }
      while (posicao >= 0)//aqui contaremos os pontos
      {
        dataGridView_Palavra.Rows[0].Cells[posicao].Value = letra.ToString();
        if (dataGridView_Palavra.Rows[0].Cells[posicao].Value.ToString() == letra.ToString())
        {
          pontos++;
          lbPontos.Text = "" + pontos;
        }

        if (posicao < palavraSorteada.Palavra.Length - 1)
        {
          var posicaoAnterior = posicao;
          posicao += palavraSorteada.Palavra.Substring(posicao + 1).IndexOf(letra.ToString()) + 1;
          if (posicao == posicaoAnterior)
          {
            posicao = -1;
          }
        }
        else
        {
          posicao = -1;
        }
      }
      if (pontos == palavraSorteada.Palavra.Trim().Length)//aqui fazemos uma condição para mostrar o método Vitoria
      {
        Vitoria();
      }
      if (erros == 8)//aqui fazemos uma condição para mostrar o método Derrota
      {
        Derrota();
      }
    }
        string str = " ";
        private void Habilita()//aqui levaremos os caracteres que representam cada número de erros para  o arduino
    {
      if (erros == 1)
        str = "A";
      if (erros == 2)
        str = "B";
      if (erros == 3)
        str = "C";
      if (erros == 4)
        str = "D";
      if (erros == 5)
        str = "E";
      if (erros == 6)
        str = "F";
      if (erros == 7)
        str = "G";
      if (erros == 8)
        str = "H";

      sp.Write(str);
    }
    private void Derrota()//este método anúncia a derrota
    {
      if (cbArduino.Checked)
      {
        str = "J";
        sp.Write(str);

      }
      string nome = txtNome.Text;
      pbErro_1.Visible = false;
      pbErro_2.Visible = true;
      pbErro_3.Visible = true;
      pbErro_4.Visible = true;
      pbErro_5.Visible = true;
      pbErro_6.Visible = true;
      pbErro_7.Visible = true;
      pbErro_8.Visible = true;
      pbMorto.Visible = true;
      pbEnforcado.Visible = true;
      tmrTempo.Stop();
      DialogResult opcaoDoUsuario = new DialogResult();
      opcaoDoUsuario = MessageBox.Show($"Infelizmente você {nome} , perdeu o jogo! Deseja reiniciar o jogo?", "Perdeu!", MessageBoxButtons.YesNo, MessageBoxIcon.None);
      if (opcaoDoUsuario == DialogResult.Yes)
      {
        Reiniciar();//aqui reiniciamos o programa
      }
      else
        Close();//aqui saimos do programa
    }
    private void Vitoria()//este método anúncia sua vitória
    {
      string nome = txtNome.Text;
      pictureBox1.Visible = false;
      pictureBox2.Visible = false;
      pictureBox3.Visible = false;
      pictureBox4.Visible = false;
      pictureBox5.Visible = false;
      pictureBox6.Visible = false;
      pictureBox7.Visible = false;
      pb_Cabeca_Vitoria.Visible = true;
      pbMorto.Visible = false;
      pbErro_1.Visible = false;
      pbErro_2.Visible = true;
      pbErro_3.Visible = true;
      pbErro_4.Visible = true;
      pbErro_5.Visible = true;
      pbErro_6.Visible = true;
      pbErro_7.Visible = true;
      pbErro_8.Visible = true;
      pb_Mao_Bandeira.Visible = true;
      pb_Bandeira_01.Visible = true;
      pb_Bandeira_02.Visible = true;
      tmrTempo.Stop();
      DialogResult opcaoDoUsuario = new DialogResult();
      opcaoDoUsuario = MessageBox.Show($"Parabéns {nome} , você venceu o jogo! Deseja reiniciar o jogo?", "Ganhou!", MessageBoxButtons.YesNo, MessageBoxIcon.None);
      if (opcaoDoUsuario == DialogResult.Yes)
      {
        Reiniciar();
      }
      else
        Close();
    }
    private void Erro()//este método deixa visivel as imagens de acordo com os erros 
    {
      if (erros == 1)
      {
          pbErro_1.Visible = true;
      }
      else if (erros == 2)
      {
          pbErro_2.Visible = true;
      }
      else if (erros == 3)
      {
          pbErro_3.Visible = true;
      }
      else if (erros == 4)
      {
          pbErro_4.Visible = true;
      }
      else if (erros == 5)
      {
          pbErro_5.Visible = true;
      }
      else if (erros == 6)
      {
          pbErro_6.Visible = true;
      }
      else if (erros == 7)
      {
          pbErro_7.Visible = true;
      }
      else if (erros == 8)
      {
          pbErro_8.Visible = true;
      }

    }

    private void Reiniciar()//este método reinicia o programa
    {
      pontos = 0;
      erros = 0;
      lbDica.Text = "";
      dataGridView_Palavra.Rows.Clear();
      dataGridView_Palavra.Refresh();
      txtNome.Clear();
      cbComDica.Checked = false;
      tmrTempo.Stop();
      lbTempo.Text = "" + "s";
      if(cbArduino.Checked)
       {
         str = "I";
         sp.Write(str);

       }
      for(int i = 0; i < botoesqtd; i++)
       {
        BotoesClicados[i].Enabled = true;
       }
      botoesqtd = 0;
      BotoesClicados = new Button[45];
      lbPontos.Text = "";
      lbErros.Text = "";

      pictureBox1.Visible = true;
      pictureBox2.Visible = true;
      pictureBox3.Visible = true;
      pictureBox4.Visible = true;
      pictureBox5.Visible = true;
      pictureBox6.Visible = true;
      pictureBox7.Visible = true;
      pb_Cabeca_Vitoria.Visible = false;
      pbMorto.Visible = false;
      pbEnforcado.Visible = false;
      pbErro_1.Visible = false;
      pbErro_2.Visible = false;
      pbErro_3.Visible = false;
      pbErro_4.Visible = false;
      pbErro_5.Visible = false;
      pbErro_6.Visible = false;
      pbErro_7.Visible = false;
      pbErro_8.Visible = false;
      pb_Mao_Bandeira.Visible = false;
      pb_Bandeira_01.Visible = false;
      pb_Bandeira_02.Visible = false;
     
    }
  }
}