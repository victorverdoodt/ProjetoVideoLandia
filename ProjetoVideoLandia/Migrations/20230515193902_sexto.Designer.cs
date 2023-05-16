﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjetoVideoLandia.Data;

#nullable disable

namespace ProjetoVideoLandia.Migrations
{
    [DbContext(typeof(VideoLandiaContext))]
    [Migration("20230515193902_sexto")]
    partial class sexto
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("ProjetoVideoLandia.Models.Ator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataDeNascimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Foto")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PaisDeNascimento")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Atores");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.Filme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Ano")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Diretor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FotoDaCapa")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sinopse")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ValorDeCusto")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Filmes");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.FilmeAtor", b =>
                {
                    b.Property<int>("FilmeId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(1);

                    b.Property<int>("AtorId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(2);

                    b.Property<string>("Personagem")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("FilmeId", "AtorId");

                    b.HasIndex("AtorId");

                    b.ToTable("FilmesAtores");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.FilmeGenero", b =>
                {
                    b.Property<int>("FilmeId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(1);

                    b.Property<int>("GeneroId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(2);

                    b.HasKey("FilmeId", "GeneroId");

                    b.HasIndex("GeneroId");

                    b.ToTable("FilmesGeneros");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.Genero", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Generos");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.MensagemContato", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Assunto")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Mensagem")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MensagensContato");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.FilmeAtor", b =>
                {
                    b.HasOne("ProjetoVideoLandia.Models.Ator", "Ator")
                        .WithMany("FilmesParticipados")
                        .HasForeignKey("AtorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetoVideoLandia.Models.Filme", "Filme")
                        .WithMany("AtoresParticipantes")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ator");

                    b.Navigation("Filme");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.FilmeGenero", b =>
                {
                    b.HasOne("ProjetoVideoLandia.Models.Filme", "Filme")
                        .WithMany("Generos")
                        .HasForeignKey("FilmeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetoVideoLandia.Models.Genero", "Genero")
                        .WithMany("FilmesDoGenero")
                        .HasForeignKey("GeneroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Filme");

                    b.Navigation("Genero");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.Ator", b =>
                {
                    b.Navigation("FilmesParticipados");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.Filme", b =>
                {
                    b.Navigation("AtoresParticipantes");

                    b.Navigation("Generos");
                });

            modelBuilder.Entity("ProjetoVideoLandia.Models.Genero", b =>
                {
                    b.Navigation("FilmesDoGenero");
                });
#pragma warning restore 612, 618
        }
    }
}