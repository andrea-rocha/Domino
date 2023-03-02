## MINIMAL API .NET CORE V6 + JWT
Con la llegada de .NET 6 y C# 10.0 podemos encontrar features muy interesantes, entre ellas la creación de minimal apis, para este proyecto se usa la plantilla de web Api para .net core y se testea y documenta la api con swagger

para este ejercicio practico se simula una base de datos virtual a traves de Repositories, en el que hay un FichaRepositorio y un UsuarioRepositorio

#### Dependecias Usadas 

- Microsoft.AspNetCore.Authentication.JwtBearer (6.0.0)
- Newtonsoft.Json (13.0.2)
- Swashbuckle.AspNetCore (6.2.3)
- System.IdentityModel.Tokens.Jwt(6.27.0)

Clone el repositorio Domino
```
git clone https://github.com/andrea-rocha/Domino.git
```
Ubicarse en el archivo appsettings.json 
```
- encontrará la configuración de la key de encriptación para el JWT, la dirección del emisor y la Audiencia en este caso con el respectivo puerto 
```
Ejecute la solucion del programa 
![swagger](./imagenes/swagger.png?raw=true)
se exponen los siguientes metodos

- /login
debera loggearse para poder tener acceso a ciertos metodos, para ello ejecute el metodo las credenciales se encuentran en la carpeta Repositories, archivo usuarioRepositorio, encontrara 2 usuarios: un usuario tipo administrador y usuario tipo standard los cuales podran ejeciutar servicios de acuerdo a sus permisos.

una vez envie en el post las credenciales de nombreUsuario y contraseña en el Response body code 200 encontra el token que debe utilizar para loggearse 

-copie el contenido dentro de las ""
-dirigase al boton de Authorize en la parte superior y pegue el token en el campo Value:

![swagger](./imagenes/inicio.png?raw=true)

esto le permitira dependiendo de las credenciales seleccionadas acceder a los servicios 
los servicios de create, update y delete estan disponibles para el usuario administrador, los demas usuarios estan disponibles para usuarios standard 

- /list 
obtine la lista de fichas en el repositorio 

- /get
obtine 1 ficha de acuerdo a su nombre los cuales estan en formato "ficha1"..ficha2 etc 

- /update 
actualiza 1 ficha en sus atributos de nombre y valores con su respectiva validación de existencia previa 

- /delete 
 elimina 1 ficha de acuerdo a su nombre de ficha 

- /OrdenarFichas
 soluciona el problema:
Implemente una forma de ordenar un conjunto dado de fichas de dominó de tal manera que se
construya una cadena correcta de fichas (los puntos en una de las mitades de una ficha concuerdan con
los puntos que tiene la mitad vecina de la ficha adyacente) y que los puntos de las mitades de aquellas
fichas que no tengan vecino (fichas primera y última) concuerden uno con el otro.

las fichas de prueba son: 

``` 
[
  {
    "nombre": "ficha1",
    "valores": [2,1]
  },
  {
    "nombre": "ficha2",
    "valores": [2,3]
  },
  {
    "nombre": "ficha3",
    "valores": [1,3]
  }
]
```
cadena Invalida 
``` 
[
  {
    "nombre": "ficha1",
    "valores": [2,1]
  },
  {
    "nombre": "ficha2",
    "valores": [4,1]
  },
  {
    "nombre": "ficha3",
    "valores": [2,3]
  }
]
```
validacion de que tenga mas de 1 ficha 
``` 
[
  {
    "nombre": "ficha1",
    "valores": [2,1]
  }
]
```
validacion de que tega menos de 6 fichas 
``` 
[
  {
    "nombre": "ficha1",
    "valores": [2,1]
  },
  {
    "nombre": "ficha2",
    "valores": [2,3]
  },
  {
    "nombre": "ficha3",
    "valores": [1,3]
  },
  {
    "nombre": "ficha4",
    "valores": [1,1]
  },
  {
    "nombre": "ficha5",
    "valores": [2,4]
  },
  {
    "nombre": "ficha6",
    "valores": [1,5]
  },
  {
    "nombre": "ficha7",
    "valores": [6,1]
  },
  {
    "nombre": "ficha8",
    "valores": [5,3]
  },
  {
    "nombre": "ficha9",
    "valores": [3,3]
  }
]
```
Gracias.