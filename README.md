<justify> En el presente proyecto se utilizo POSTMAN para las pruebas en la API, ademas se solicita usar OAuth o JWT en este caso se utilizo JWT por medio de un usuario predefinido para poder acceder a servicios con el uso de formularios, opciones como llenar encuesta no solicitan el uso de este usuario, se trabajo en una base de datos relacional SQL SERVER conjuntamente con el lenguaje de C# Asp.Net  </justify>

<h1>Collection en POSTMAN</h1>

https://elements.getpostman.com/redirect?entityId=28685331-41026e4b-a4cb-4a5b-b1a3-42fe34ea6277&entityType=collection


<h1>Script BD</h1>

```
create database prueba_tecnica;
use prueba_tecnica;

create table formulario(
link_formulario varchar(2000) primary key not null,
nombre_encuesta varchar(100) not null,
descripcion_encuesta varchar(250) not null,
);


 create table campo(
id int identity(1,1) primary key not null,
nombre_campo varchar(100) not null,
 titulo_campo varchar(100) not null,
 esrequerido char not null,
tipo_campo varchar(10) NOT NULL,
link_formulario varchar(2000) not null
 );
 
 create table campo_en_formulario(
 id int identity(1,1)primary key not null,
 link_formulario varchar(2000) not null,
 id_campo int not null,
 valor varchar(2000),
 foreign key(link_formulario) references formulario(link_formulario),
 foreign key(id_campo) references campo(id),
 ) 

```
