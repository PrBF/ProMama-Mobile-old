﻿using ProMama.Models;
using ProMama.ViewModels.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProMama.ViewModels.Home.Paginas
{
    class MarcoVisualizacaoViewModel : ViewModelBase
    {
        private Aplicativo app = Aplicativo.Instance;

        private Marco Marco { get; set; }

        private string _titulo;
        public string Titulo
        {
            get
            {
                return _titulo;
            }
            set
            {
                _titulo = value;
                Notify("Titulo");
            }
        }

        private ImageSource _imagem;
        public ImageSource Imagem
        {
            get
            {
                return _imagem;
            }
            set
            {
                _imagem = value;
                Notify("Imagem");
            }
        }

        private bool _alcancado;
        public bool Alcancado
        {
            get
            {
                return _alcancado;
            }
            set
            {
                _alcancado = value;
                Notify("Alcancado");
            }
        }

        private bool _naoAlcancado;
        public bool NaoAlcancado
        {
            get
            {
                return _naoAlcancado;
            }
            set
            {
                _naoAlcancado = value;
                Notify("NaoAlcancado");
            }
        }

        private string _textoAlcancado;
        public string TextoAlcancado
        {
            get
            {
                return _textoAlcancado;
            }
            set
            {
                _textoAlcancado = value;
                Notify("TextoAlcancado");
            }
        }

        private string _textoNaoAlcancado;
        public string TextoNaoAlcancado
        {
            get
            {
                return _textoNaoAlcancado;
            }
            set
            {
                _textoNaoAlcancado = value;
                Notify("TextoNaoAlcancado");
            }
        }

        private bool _extraAparece;
        public bool ExtraAparece
        {
            get
            {
                return _extraAparece;
            }
            set
            {
                _extraAparece = value;
                Notify("ExtraAparece");
            }
        }

        private bool _textoExtraAparece;
        public bool TextoExtraAparece
        {
            get
            {
                return _textoExtraAparece;
            }
            set
            {
                _textoExtraAparece = value;
                Notify("TextoExtraAparece");
            }
        }

        private string _textoExtra;
        public string TextoExtra
        {
            get
            {
                return _textoExtra;
            }
            set
            {
                _textoExtra = value;
                Notify("TextoExtra");
            }
        }

        public string ExtraInput { get; set; }

        private Keyboard _extraInputKeyboard;
        public Keyboard ExtraInputKeyboard
        {
            get
            {
                return _extraInputKeyboard;
            }
            set
            {
                _extraInputKeyboard = value;
                Notify("ExtraInputKeyboard");
            }
        }

        private bool _dataAparece;
        public bool DataAparece
        {
            get
            {
                return _dataAparece;
            }
            set
            {
                _dataAparece = value;
                Notify("DataAparece");
            }
        }

        private DateTime _dataMinima;
        public DateTime DataMinima
        {
            get
            {
                return _dataMinima;
            }
            set
            {
                _dataMinima = value;
                Notify("DataMinima");
            }
        }

        private DateTime _dataMaxima;
        public DateTime DataMaxima
        {
            get
            {
                return _dataMaxima;
            }
            set
            {
                _dataMaxima = value;
                Notify("DataMaxima");
            }
        }

        private DateTime _dataSelecionada;
        public DateTime DataSelecionada
        {
            get
            {
                return _dataSelecionada;
            }
            set
            {
                _dataSelecionada = value;
                Notify("DataSelecionada");
            }
        }

        private INavigation Navigation { get; set; }
        private readonly INavigationService NavigationService;
        private readonly IMessageService MessageService;

        public ICommand SalvarCommand { get; set; }

        public MarcoVisualizacaoViewModel(INavigation _navigation, Marco _marco)
        {
            Navigation = _navigation;
            NavigationService = DependencyService.Get<INavigationService>();
            MessageService = DependencyService.Get<IMessageService>();

            DataMinima = app._crianca.crianca_dataNascimento;
            DataMaxima = DateTime.Now;
            DataSelecionada = DateTime.Now;

            Marco = _marco;

            Titulo = Marco.Titulo;
            Imagem = Marco.Imagem;
            Alcancado = Marco.Alcancado;
            NaoAlcancado = !Alcancado;

            if (Alcancado)
            {
                switch (Marco.marco)
                {
                    case 1:
                        TextoAlcancado =
                            "O primeiro dentinho de " + app._crianca.crianca_primeiro_nome + " nasceu em " + Marco.data + "!";
                        break;
                    case 2:
                        TextoAlcancado = app._crianca.crianca_sexo == 0 ?
                            app._crianca.crianca_primeiro_nome + " virou-se sozinho pela primeira vez em " + Marco.data + "!" :
                            app._crianca.crianca_primeiro_nome + " virou-se sozinha pela primeira vez em " + Marco.data + "!";
                        break;
                    case 3:
                        TextoAlcancado = app._crianca.crianca_sexo == 0 ?
                            app._crianca.crianca_primeiro_nome + " sentou-se sozinho pela primeira vez em " + Marco.data + "!" :
                            app._crianca.crianca_primeiro_nome + " sentou-se sozinha pela primeira vez em " + Marco.data + "!";
                        break;
                    case 4:
                        TextoAlcancado =
                            app._crianca.crianca_primeiro_nome + " parou de se alimentar exclusivamente de leite materno aos " + Marco.extra + " meses!";
                        break;
                    case 5:
                        TextoAlcancado =
                            app._crianca.crianca_primeiro_nome + " comeu sua primeira fruta em " + Marco.data + "!";
                        break;
                    case 6:
                        TextoAlcancado =
                            app._crianca.crianca_primeiro_nome + " comeu sua primeira papa salgada em " + Marco.data + "!";
                        break;
                    case 7:
                        TextoAlcancado =
                            app._crianca.crianca_primeiro_nome + " engatinhou pela primeira vez em " + Marco.data + "!";
                        break;
                    case 8:
                        TextoAlcancado =
                            app._crianca.crianca_primeiro_nome + " falou pela primeira vez em " + Marco.data + ", e sua primeira palavra foi " + Marco.extra + "!";
                        break;
                    case 9:
                        TextoAlcancado =
                            app._crianca.crianca_primeiro_nome + " deu seus primeiros passos em " + Marco.data + "!";
                        break;
                }
            }
            else
            {
                TextoExtraAparece = false;
                ExtraAparece = false;
                DataAparece = true;

                switch (Marco.marco)
                {
                    case 1:
                        TextoNaoAlcancado =
                            "Quando foi que nasceu o primeiro dentinho de " + app._crianca.crianca_primeiro_nome + "?";
                        break;
                    case 2:
                        TextoNaoAlcancado = app._crianca.crianca_sexo == 0 ?
                            "Quando foi a primeira vez que " + app._crianca.crianca_primeiro_nome + " virou-se sozinho?" :
                            "Quando foi a primeira vez que " + app._crianca.crianca_primeiro_nome + " virou-se sozinha?";
                        break;
                    case 3:
                        TextoNaoAlcancado = app._crianca.crianca_sexo == 0 ?
                            "Quando foi a primeira vez que " + app._crianca.crianca_primeiro_nome + " sentou-se sozinho?" :
                            "Quando foi a primeira vez que " + app._crianca.crianca_primeiro_nome + " sentou-se sozinha?";
                        break;
                    case 4:
                        DataAparece = false;
                        ExtraAparece = true;
                        ExtraInputKeyboard = Keyboard.Numeric;
                        TextoNaoAlcancado =
                            "Com quantos meses " + app._crianca.crianca_primeiro_nome + " parou de se alimentar exclusivamente de leite materno?";
                        break;
                    case 5:
                        TextoNaoAlcancado =
                            "Quando foi que " + app._crianca.crianca_primeiro_nome + " comeu sua primeira fruta?";
                        break;
                    case 6:
                        TextoNaoAlcancado =
                            "Quando foi que " + app._crianca.crianca_primeiro_nome + " comeu sua primeira papa salgada?";
                        break;
                    case 7:
                        TextoNaoAlcancado =
                            "Quando foi que " + app._crianca.crianca_primeiro_nome + " engatinhou pela primeira vez?";
                        break;
                    case 8:
                        ExtraAparece = true;
                        ExtraInputKeyboard = Keyboard.Text;
                        TextoExtraAparece = true;
                        TextoExtra = "E qual foi a primeira palavra?";
                        TextoNaoAlcancado =
                            "Quando foi que " + app._crianca.crianca_primeiro_nome + " falou pela primeira vez?";
                        break;
                    case 9:
                        TextoNaoAlcancado =
                            "Quando foi que " + app._crianca.crianca_primeiro_nome + " deu os primeiros passos?";
                        break;
                }
            }

            SalvarCommand = new Command(Salvar);
        }

        private async void Salvar()
        {
            if ((ExtraAparece && !String.IsNullOrEmpty(ExtraInput)) || !ExtraAparece)
            {
                Marco.crianca = app._crianca.crianca_id;
                Marco.data = DataSelecionada.ToString("dd/MM/yyyy");
                Marco.extra = ExtraInput;
                Marco.Alcancado = true;
                App.MarcoDatabase.SaveIncrementing(Marco);
                await Navigation.PopAsync();
            }
            else
            {
                await MessageService.AlertDialog("Nenhum campo pode estar vazio.");
            }
        }
    }
}