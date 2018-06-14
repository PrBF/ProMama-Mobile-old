﻿using ProMama.Models;
using ProMama.Views.Home;
using ProMama.Views.Home.Paginas;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ProMama.ViewModels.Home
{
    class HomeMasterViewModel : ViewModelBase
    {
        private Aplicativo app = Aplicativo.Instance;

        private string _nome;
        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                _nome = value;
                Notify("Nome");
            }
        }

        private string _idade;
        public string Idade
        {
            get
            {
                return _idade;
            }
            set
            {
                _idade = value;
                Notify("Idade");
            }
        }

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

        private ObservableCollection<HomeMenuItem> _menuItems;
        public ObservableCollection<HomeMenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                _menuItems = value;
                Notify("MenuItems");
            }
        }

        public HomeMasterViewModel()
        {
            app._master = this;
            Load();
        }

        public void Load(){
            Nome = app._crianca.crianca_primeiro_nome;
            Idade = app._crianca.IdadeExtenso;
            Foto = App.FotoDatabase.GetMostRecent(app._crianca.crianca_id);

            Type FaleConoscoType = null;
            if (App.BairroDatabase.Find(app._usuario.bairro).bairro_nome.Equals("Outro"))
                FaleConoscoType = typeof(FaleConoscoOutrosView);
            else
                FaleConoscoType = typeof(FaleConoscoView);

            MenuItems = new ObservableCollection<HomeMenuItem>(new[]
                {
                    new HomeMenuItem(0, "Início", "fa-home", typeof(HomeDetail)),
                    new HomeMenuItem(1, "Perfil da Criança", "fa-child", typeof(PerfilCriancaView)),
                    new HomeMenuItem(2, "Perfil da Mãe", "fa-user", typeof(PerfilMaeView)),
                    new HomeMenuItem(3, "Galeria", "fa-image", typeof(GaleriaView)),
                    new HomeMenuItem(4, "Acompanhamento da Criança", "fa-table", typeof(AcompanhamentoView)),
                    new HomeMenuItem(5, "Marcos do Desenvolvimento", "fa-trophy", typeof(MarcosView)),
                    new HomeMenuItem(6, "Fale Conosco", "fa-comments", FaleConoscoType),
                    new HomeMenuItem(7, "Dúvidas Frequentes", "fa-question-circle", typeof(DuvidasFrequentesView)),
                    new HomeMenuItem(8, "Postos de Saúde", "fa-map", typeof(HomeDetail)),
                    new HomeMenuItem(9, "Redes Sociais", "fa-globe", typeof(HomeDetail)),
                    new HomeMenuItem(10, "Selecionar Criança", "fa-exchange", typeof(SelecionarCriancaView)),
                    new HomeMenuItem(11, "Sair", "fa-sign-out", typeof(LogoutView))
                }
            );
        }
    }
}