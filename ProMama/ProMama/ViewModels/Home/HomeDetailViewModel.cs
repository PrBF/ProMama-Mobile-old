﻿using Plugin.Connectivity;
using ProMama.Components;
using ProMama.Models;
using ProMama.ViewModels.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProMama.ViewModels.Home
{
    class HomeDetailViewModel : ViewModelBase
    {
        private Aplicativo app = Aplicativo.Instance;

        // Criança
        public string Nome { get; set; }

        // Foto da Criança
        private ImageSource _foto { get; set; }
        public ImageSource Foto
        {
            get
            {
                return _foto;
            }
            set
            {
                _foto = value;
                Notify("Foto");
            }
        }

        // Variavéis auxiliares para controle da timeline
        private List<string> IdadesExtensoLista { get; set; }
        private Dictionary<double, int> IdadeAuxIndexador { get; set; }

        private int _idadeAuxIndex;
        public int IdadeAuxIndex
        {
            get
            {
                return _idadeAuxIndex;
            }
            set
            {
                _idadeAuxIndex = value;
                IdadeExtenso = IdadesExtensoLista[value];
                OrganizaSetas();
                OrganizaInformacoes();
                DefineFoto();
            }
        }

        private string _idadeExtenso;
        public string IdadeExtenso
        {
            get { return _idadeExtenso; }
            set
            {
                _idadeExtenso = value;
                Notify("IdadeExtenso");
            }
        }

        private string _setaEsquerdaCor;
        public string SetaEsquerdaCor
        {
            get
            {
                return _setaEsquerdaCor;
            }
            set
            {
                _setaEsquerdaCor = value;
                Notify("SetaEsquerdaCor");
            }
        }

        private string _setaDireitaCor;
        public string SetaDireitaCor
        {
            get
            {
                return _setaDireitaCor;
            }
            set
            {
                _setaDireitaCor = value;
                Notify("SetaDireitaCor");
            }
        }

        // Informações
        public ObservableCollection<Informacao> Informacoes { get; set; }

        private List<Informacao> InformacoesAux = new List<Informacao>();

        // Picker
        public List<string> IdadesPickerLista { get; set; }

        // Commands
        public ICommand MenosIdadeCommand { get; set; }
        public ICommand MaisIdadeCommand { get; set; }
        public ICommand IdadePickerCommand { get; set; }
        public ICommand AbrirInformacaoCommand { get; set; }
        public ICommand AtualizarInformacoesCommand { get; set; }
        public ICommand GaleriaCommand { get; set; }

        // Navigation
        private INavigation Navigation { get; set; }
        private readonly INavigationService NavigationService;

        // Rest
        private readonly IRestService RestService;

        // Construtor
        public HomeDetailViewModel(INavigation _navigation)
        {
            Plugin.LocalNotifications.CrossLocalNotifications.Current.Show("title", "body");
            Plugin.LocalNotifications.CrossLocalNotifications.Current.Show("title", "body", 101, System.DateTime.Now.AddSeconds(5));

            // Salva o login
            Config cfg = new Config(app._usuario, app._crianca);
            App.ConfigDatabase.Save(cfg);

            // Informações
            RestService = DependencyService.Get<IRestService>();
            Informacoes = new ObservableCollection<Informacao>();
            InformacoesRead();

            // Lista auxiliar de idades
            DefineIdadeAuxIndexador();

            // Lista de idades por extenso
            IdadesExtensoLista = new List<string>() {
                "recém-nascido",
                "1 semana",
                "2 semanas",
                "3 semanas",
                "1 mês",
                "2 meses",
                "3 meses",
                "4 meses",
                "5 meses",
                "6 meses",
                "7 meses",
                "8 meses",
                "9 meses",
                "10 meses",
                "11 meses",
                "1 ano",
                "1 ano e 1 mês",
                "1 ano e 2 meses",
                "1 ano e 3 meses",
                "1 ano e 4 meses",
                "1 ano e 5 meses",
                "1 ano e 6 meses",
                "1 ano e 7 meses",
                "1 ano e 8 meses",
                "1 ano e 9 meses",
                "1 ano e 10 meses",
                "1 ano e 11 meses",
                "2 anos"
            };

            // Criança
            Nome = app._crianca.crianca_primeiro_nome;
            IdadeAuxIndex = IdadesExtensoLista.IndexOf(app._crianca.DefineIdadeExtenso());
            // bug-proof
            /*if (SetaDireitaCor.Equals("#EEEEEE"))
                IdadeAuxIndex--;*/

            // Idades picker
            IdadesPickerLista = new List<string>();
            for (int i = 0; i <= IdadesExtensoLista.IndexOf(app._crianca.IdadeExtenso); i++)
            {
                IdadesPickerLista.Add(IdadesExtensoLista[i]);
            }

            // Commands
            MenosIdadeCommand = new Command(MenosIdade);
            MaisIdadeCommand = new Command(MaisIdade);
            IdadePickerCommand = new Command<Picker>(IdadePicker);
            AbrirInformacaoCommand = new Command<Informacao>(AbrirInformacao);
            GaleriaCommand = new Command(Galeria);

            // Navigation
            Navigation = _navigation;
            NavigationService = DependencyService.Get<INavigationService>();

            // Sincronizando banco em thread
            Task.Run(async () =>
            {
                if (!app.onThread)
                {
                    app.onThread = true;
                    Debug.WriteLine("INÍCIO DA TENTATIVA DE SINCRONIZAÇÃO EM THREAD");

                    if (CrossConnectivity.Current.IsConnected)
                    {
                        await Ferramentas.SincronizarBanco();
                    }

                    app.onThread = false;
                    Debug.WriteLine("FIM DA TENTATIVA DE SINCRONIZAÇÃO EM THREAD");
                }
            });
        }

        // Botão da seta pra direita
        private void MaisIdade()
        {
            if (IdadeAuxIndex < 27 && IdadeAuxIndex < app._crianca.IdadeMeses + 2 && IdadesExtensoLista.IndexOf(app._crianca.IdadeExtenso) != 0)
            {
                IdadeAuxIndex++;
            }
        }

        // Botão da seta pra esquerda
        private void MenosIdade()
        {
            if (IdadeAuxIndex > 0 && IdadesExtensoLista.IndexOf(app._crianca.IdadeExtenso) != 0)
            {
                IdadeAuxIndex--;
            }
        }

        // Mostra picker de idades
        private void IdadePicker(Picker p)
        {
            p.Focus();
        }

        // Organiza o display as setas
        private void OrganizaSetas()
        {
            if (IdadesExtensoLista.IndexOf(app._crianca.IdadeExtenso) == 0)
            {
                SetaEsquerdaCor = "#FF8A80";
                SetaDireitaCor = "#FF8A80";
            } else
            {
                if (IdadeAuxIndex == 0)
                {
                    SetaEsquerdaCor = "#FF8A80";
                    SetaDireitaCor = "#EEEEEE";
                }
                else if (IdadeAuxIndex == 27 || IdadeAuxIndex == IdadesExtensoLista.IndexOf(app._crianca.IdadeExtenso))
                {
                    SetaEsquerdaCor = "#EEEEEE";
                    SetaDireitaCor = "#FF8A80";
                }
                else
                {
                    SetaEsquerdaCor = "#EEEEEE";
                    SetaDireitaCor = "#EEEEEE";
                }
            }
        }

        // Organiza as informações mostradas na tela de acordo com a idade que o usuário escolhe ao interagir com as setas
        private void OrganizaInformacoes()
        {
            foreach (var info in InformacoesAux)
            {
                KeyValuePair<double, int> idade = IdadeAuxIndexador.FirstOrDefault(x => x.Key == info.informacao_idadeSemanasInicio);

                if ((idade.Value == IdadeAuxIndex) && (app._crianca.IdadeSemanas >= info.informacao_idadeSemanasInicio))
                    AdicionarInfo(info);
                else
                    RemoverInfo(info);
            }
        }
        
        // Abre pagina de informação
        private async void AbrirInformacao(Informacao informacao)
        {
            await NavigationService.NavigateInformacao(Navigation, informacao);
        }

        private void Galeria()
        {
            app._home.Detail_Galeria();
        }

        private void InformacoesRead()
        {
            var count = 0;
            var informacoes = App.InformacaoDatabase.GetAll();
            foreach (var i in informacoes)
            {
                
                InformacoesAux.Add(i);
                count++;
            }
        }

        private void AdicionarInfo(Informacao info)
        {
            if (!Informacoes.Contains(info))
                Informacoes.Add(info);
        }

        private void RemoverInfo(Informacao info)
        {
            if (Informacoes.Contains(info))
                Informacoes.Remove(info);
        }

        private void DefineIdadeAuxIndexador()
        {
            IdadeAuxIndexador = new Dictionary<double, int>()
            {
                {0, 0},
                {0.31037428571429, 0},
                {0.46556142857143, 0},
                {0.62074857142857, 0},
                {0.77593571428571, 0},
                {0.93112285714286, 0},
                {1.08631, 1},
                {2.17262, 2},
                {3.25893, 3},
                {4.34524, 4},
                {5.43155, 4},
                {6.51786, 4},
                {7.60417, 4},
                {8.69048, 5},
                {9.77679, 5},
                {10.8631, 5},
                {11.94941, 5},
                {13.03572, 6},
                {14.12203, 6},
                {15.20834, 6},
                {16.29465, 6},
                {17.38096, 7},
                {18.46727, 7},
                {19.55358, 7},
                {20.63989, 7},
                {21.7262, 8},
                {22.81251, 8},
                {23.89882, 8},
                {24.98513, 8},
                {26.07144, 9},
                {27.15775, 9},
                {28.24406, 9},
                {29.33037, 9},
                {30.41668, 10},
                {31.50299, 10},
                {32.5893, 10},
                {33.67561, 10},
                {34.76192, 11},
                {35.84823, 11},
                {36.93454, 11},
                {38.02085, 11},
                {39.10716, 12},
                {40.19347, 12},
                {41.27978, 12},
                {42.36609, 12},
                {43.4524, 12},
                {44.53871, 13},
                {45.62502, 13},
                {46.71133, 13},
                {47.79764, 13},
                {48.88395, 14},
                {49.97026, 14},
                {51.05657, 14},
                {52.14288, 14},
                {56.48812, 15},
                {60.83336, 16},
                {65.1786, 17},
                {69.52384, 18},
                {73.86908, 19},
                {78.21432, 20},
                {82.55956, 21},
                {86.9048, 22},
                {91.25004, 23},
                {95.59528, 24},
                {99.94052, 25},
                {104.28576, 26}
            };
        }

        private int DefineIdadeAux()
        {
            if (app._crianca.IdadeExtenso.Equals("2 anos"))
                return IdadesExtensoLista.Count() - 1;

            var idadeSemanas = app._crianca.IdadeSemanas;

            for (var i=0; i<IdadeAuxIndexador.Count()-1; i++)
            {
                var firstPair = IdadeAuxIndexador.ElementAt(i);
                var secondPair = IdadeAuxIndexador.ElementAt(i+1);
                if (firstPair.Key <= idadeSemanas && secondPair.Key > idadeSemanas)
                {
                    return firstPair.Value;
                }
            }

            return IdadesExtensoLista.Count() - 1;
        }

        private void DefineFoto()
        {
            var list = App.FotoDatabase.GetAllByChildId(app._crianca.crianca_id);
            if (list.Count() == 0)
            {
                Foto = "avatar_default.png";
                return;
            }

            if (IdadeAuxIndex < 4)
            {
                foreach (var f in list)
                {
                    if (f.mes == 0)
                    {
                        Foto = f.caminho;
                        return;
                    }
                }
                Foto = "avatar_default.png";
            } else
            {
                foreach (var f in list)
                {
                    if (f.mes == IdadeAuxIndex - 3)
                    {
                        Foto = f.caminho;
                        return;
                    }
                }
                Foto = "avatar_default.png";
            }
        }
    }
}
