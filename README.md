![image alt text](assets/GDD/Title.png)

# Game Maker Maker

## Game Design Document


# Índice

* 1\. [Introduccion](#introduccion)
  + 1.1\. [Concepto](#Concepto)
  + 1.2\. [Características principales](#Caracteristicasprincipales)
  + 1.3\. [Género](#Genero)
  + 1.4\. [Estilo Visual](#EstiloVisual)
  + 1.5\. [Alcance](#Alcance)
  + 1.6\. [Jugabilidad](#Jugabilidad)
  + 1.7\. [Propósito y público objetivo](#Propositoypublicoobjetivo)
* 2\. [Mecánicas del juego](#mecanicas)
  * 2.1\. [Jugabilidad](#Jugabilidad)
  * 2.2\. [Props](#Trampas)
  * 2.3\. [Puntos de interacción](#Puntosdeinteraccion)
  * 2.4\. [Flujo de juego (Game Loop)](#FlujodejuegoGameLoop)
  * 2.5\. [Movimiento](#Movimiento)
  * 2.6\. [Cámara](#Camara)
  * 2.7\. [Controles](#Controles)
  * 2.8\. [Niveles](#Niveles)
* 3\. [Interfaz](#interfaz)
  * 3.1\. [Diagrama de Flujo](#DiagramadeFlujo)
  * 3.2\. [Pantalla de Nivel (HUD)](#PantalladeNivel)

* 4\. [Arte y Sonido](#arteysonido)
  * 4.1\. [Arte](#Arte)
  * 4.2\. [Sonido](#Sonido)
* 5\. [Roles](#roles)

<div id="introduccion"></ol>

# **1. Introducción**

*Game Maker Maker* es un videojuego para *navegador y dispositivos móviles*, con estética 3D, desarrollado mediante el motor de videojuegos Unity. En este documento se van a plasmar los aspectos relacionados con el diseño fundamental del videojuego, de forma que nos sirva como carta de presentación ante futuros colaboradores.

<div id="Concepto"></ol>

## **1.1. Concepto**

*Todos conocemos las setas de Super Mario, las Pokebolas de Pokémon, o las frutas del Pac-Man, pero ¿de dónde vienen? En Game Maker Maker, ¡tenemos la respuesta! ¡Ponte en  los zapatos de un empleado de la fábrica mundial de props, y prepárate para dar vida a todas esas sagas que tanto adoras! Pero, cuidado: no va a ser tarea fácil. Necesitarás mucha rapidez y coordinación. ¿Estarás a la altura del reto que supone ser un verdadero creador de videojuegos?*

El juego nos pone en los zapatos de un empleado de una **fábrica de "props" de videojuegos,** es decir, de todos aquellos objetos (decorativos o no) que se utilizan para su desarrollo (ejemplo: una seta de *Super Mario*, una mesa, un arma, etc.).  Así, la intención del juego será proponer una idea graciosa y atractiva, en la que muchos jugadores puedan reconocer guiños a sus sagas favoritas, al mismo tiempo que experimentan una jugabilidad muy similar a la de juegos como *Overcooked!.*

Comenzaremos en un pequeño escenario, visto desde arriba, donde tendremos una **cinta transportadora** (por la que van llegando objetos para ser tratados), varios **puntos de preparación** para llevar a cabo el tratamiento, y mesas vacías para depositar objetos temporalmente. De igual manera, se dispondrán de un par de **puntos de entrega** de props, dependiendo del videojuego al que pertenezcan.

Al empezar la partida, comienzan a llegar objetos por la cinta. El jugador debe recogerlos, introducirlos en la estación de preparación correspondiente (según el objeto), y entregarlos a la estación del videojuego al que pertenecen. Si, durante el proceso, **alguno de los objetos de la cinta no es recogido, perderá una de las vidas de las que dispone** (al perder todas, fin de la partida). Si, por el contrario, consigue **preparar y entregar todos los props, completará el nivel satisfactoriamente** y podrá avanzar al siguiente, que será un mayor reto de dificultad.

Para elaborar los props, deberá atenderse a la **"receta"** de cada uno, es decir, al conjunto de pasos necesarios para elaborarlo. Así, por ejemplo, un prop necesitará pasar por la estación de tratamiento A y B, mientras otro sólo necesitará pasar por la A. 

El diseño de los puntos de preparación será lo más intuitivo posible para permitir que el jugador pueda inferir en cuál debería introducir el prop, pero, a fin de recompensar la memoria y atención del jugador, se dispondrá de un **"libro de recetas"** en el cual se especificará el proceso de elaboración de cada prop. Dicho libro se visualizará una vez al empezar la partida, y posteriormente, yendo a un punto clave del escenario donde se situará (ir a mirarlo en medio de la partida implicará perder tiempo y dejar desatendida la cinta transportadora, por lo que supondrá un reto intentar recordar las recetas, o intuirlas).

<div id="Caracteristicasprincipales"></ol>


## **1.2. Características principales**

* **Controles sencillos:** los controles del juego consistirán en un sistema **"Point & Click",** en el que el usuario tocará la pantalla de su móvil (o hará click con el ratón) en el punto al que desea moverse, el objeto que desea recoger, o el elemento con el que desea interactuar. Al hacerlo, el personaje comenzará a desplazarse hasta el punto marcado y realizar la acción determinada. Si, en el proceso, se toca o selecciona otro punto, la acción anterior quedará anulada y se realizará la nueva. 

* **Cámara:** la cámara estará situada de forma que **se contemple todo el nivel desde arriba,** de forma casi cenital, pero con un ángulo de inclinación suficiente como para poder apreciar el 3D (no es totalmente perpendicular al suelo). Su comportamiento será estático, de forma que apenas tendrá movimiento durante el juego.

* **Ritmo dinámico: **las mecánicas básicas del juego dan lugar a un** ritmo ágil y rápido**, de forma que el jugador nunca experimente un momento aburrido o predecible, si no que el juego le mantenga alerta y con ganas de superar nuevos retos.

* **Jugabilidad extensible**: sistema de niveles y flujo de juego acorde a ellos, lo cual otorga al juego de alta escalabilidad para añadir contenido en un futuro.

* **Temática divertida:** la naturaleza de la premisa del juego, así como de la sencillez de sus mecánicas, dan lugar a una dinámica final que busca provocar la sonrisa en el jugador, al mismo tiempo que atraparle en su jugabilidad.

<div id="Genero"></ol>


## **1.3. Género**

El género del juego es de tipo **"Simulación" o “Arcade”,** donde el personaje debe completar una serie de tareas dinámicas, con un ritmo rápido que le impide acomodarse o permanecer ocioso, y con dificultad progresiva en sus niveles, como una cadena de montaje. El ejemplo más claro que se adapta a la fórmula del juego es *Overcooked!*, aunque en general se inspira en aspectos comunes en los juegos de índole “for fun”.

<div id="EstiloVisual"></ol>


## **1.4. Estilo Visual**

La estética tendrá un estilo visual de **temática cartoon,** no realista, y será generalmente de aspecto amigable, colores pintorescos y animaciones exageradas, y contará con un arte propio y diferenciable.

A modo de parodia al desarrollo de videojuegos en general, algunos elementos de la estética buscarán imitar un "blockout" 3D, por ejemplo, el personaje principal (que se llama *GameObject*) tendrá una apariencia goofy e infantil, muy similar a un personaje de CAT (basado en cubos y primitivas de colores), etc. Esto supondrá un contraste con los props más elaborados de las sagas que el juego comprende, de forma que se realza la naturaleza cómica del juego.

<div id="Alcance"></ol>


## **1.5. Alcance**

Debido a que el juego está planteado por niveles, dispondrá de una gran escalabilidad, de forma que siempre se podrán añadir niveles diferentes, con distintas distribuciones de sus elementos en el espacio, juegos y props nuevos.

<div id="Jugabilidad"></ol>


## **1.6. Jugabilidad**

Los niveles serán realizados de forma progresiva y tendrán distintos niveles de dificultad. Las características principales de los niveles serán:

* **Dificultad incremental:** los niveles avanzados presentarán un reto mayor, debido a un número incrementado de elementos que gestionar, y al propio diseño del nivel.

* **Diferentes props:** Cada nivel dispondrá de un número de props diferente. De esta manera, por ejemplo, el primer nivel tendrá 4 props, de 2 juegos diferentes, y el segundo, 8 props, de 4 juegos diferentes.

* **Controles "Point & Click":** los controles, como se ha mencionado anteriormente, junto a la cámara aérea, permitirán visualizar y manipular el nivel en su completitud, posibilitando al usuario gestionar todos los props, puntos de interacción y demás elementos del escenario.

<div id="Propositoypublicoobjetivo"></ol>


## **1.7. Propósito y público objetivo**

Nuestro objetivo es obtener un juego 3D, de estética atractiva y llamativa, rápido, dinámico y relativamente simple (lo suficiente para ser jugable en la pantalla de un dispositivo móvil), que sirva para entretener durante los momentos aburridos del jugador.

Es un juego **apto para todos los públicos,** tanto para los amantes de los juegos de móvil como para un perfil de jugador más ocasional o "casual", y disfrutable en cualquier momento (en viajes, en casa, etc). 

Nuestro objetivo sería cumplir un **PEGI 3,** puesto que el juego será apto para todas las edades, sin lenguaje soez o desnudez explícita. La "violencia" del juego será mínima y estará limitada a un contexto de tipo dibujo cómico animado (como pueden ser los dibujos de *Tom y Jerry*), por lo que niños de todas las edades podrán disfrutar del juego.

<div id="mecanicas"></ol>


# **2. Mecánicas del juego**

En esta sección, trataremos todas las cuestiones referentes al funcionamiento básico y avanzado del juego, así como los diferentes props y elementos involucrados en él, y se explicarán controles, mapa y desarrollo de una "partida típica".

<div id="Jugabilidad"></ol>


## **2.1. Jugabilidad**

* **Niveles:** los niveles del juego consistirán en una planta de montaje de la fábrica de props, con cintas transportadoras, elementos de almacenaje y elaboración, etc. En cada nivel, dispondremos de un **número determinado de props que deberemos completar para superarlo, con un número específico de vidas.** Si un prop no es recogido de la cinta transportadora (caerá a la papelera situada al final de la misma), se perderá una vida.

* **Dificultad:** la dificultad irá aumentando con cada nivel, de forma que un nivel más avanzado tendrá una distribución más compleja y complicada de navegar, un mayor número de props y sagas a las que pertenecen, y más complejidad en las "recetas" de éstos.

* **Puntos de interacción:** los niveles tendrán diferentes puntos de preparado y entrega de props. 

Cuando un prop es introducido en un **punto de preparado,** se comienza a tratar, mostrando un pequeño indicador radial, superpuesto en el elemento, de cuánto falta para finalizar la elaboración. Al terminar, deberá ser recogido inmediatamente, pues de lo contrario la estación se sobrecargará, quedando inutilizada unos segundos y perdiendo el prop que contiene (de forma similar a si se quemase una sartén en *Overcooked!*).

Una vez preparado, cuando haya pasado por todos los puntos necesarios según su "receta", deberemos llevar el resultado a un **punto de entrega**, y depositarlo en el que corresponda, en función de su saga. Si se deposita en el punto de entrega equivocado, dicho punto quedará inutilizado unos instantes, y perderemos el prop.

Una descripción detallada se encontrará en la sección *2.3. Puntos de interacción*.

* **Props:** los niveles tendrán diferentes props, de sagas conocidas, a modo de guiño gracioso y atractivo para el usuario, que deberá recogerlos de la cinta transportadora, y llevarlos a los puntos de preparación correspondientes, para posteriormente recogerlos de éstas y depositarlos en las de entrega que encajen con el juego al que pertenece. 

Si el usuario tiene sobrecarga de props, y necesita recoger alguno de la cinta para no perderlo, puede depositarlos en diferentes "mesas" dispuestas por el nivel (abundantes, pero limitadas).

Una descripción detallada se encontrará en la sección *2.2. Props*.

* **Progresión del jugador:** para incentivar la rejugabilidad y búsqueda de un reto por parte del jugador, se incorporará un sistema de puntuación, animandole a jugar a otra partida para superar su puntuación o la de sus amigos. Este sistema tendrá en cuenta diversos factores: el tiempo que se ha tardado en completar el nivel, número de props entregados en la saga correcta, número de puntos de preparado sobrecargados, etc.

<div id="Props"></ol>


## **2.2. Props**

A continuación se detallan los props utilizados en el videojuego, con los datos respectivos de cada uno:

<table>
  <tr>
    <td>Nombre</td>
    <td>Saga</td>
    <td>Nivel</td>
  </tr>
  <tr>
    <td>Champiñón rojo</td>
    <td>Super Mario Bros</td>
    <td>1</td>
  </tr>
  <tr>
    <td>Caparazón verde</td>
    <td>Super Mario Bros</td>
    <td>1</td>
  </tr>
  <tr>
    <td>Cereza</td>
    <td>Pac-Man</td>
    <td>1</td>
  </tr>
  <tr>
    <td>Manta de fantasma</td>
    <td>Pac-Man</td>
    <td>1</td>
  </tr>
  <tr>
    <td>Cubo de compañía</td>
    <td>Portal</td>
    <td>2</td>
  </tr>
  <tr>
    <td>Torreta</td>
    <td>Portal</td>
    <td>2</td>
  </tr>
  <tr>
    <td>Pokeball</td>
    <td>Pokémon</td>
    <td>2</td>
  </tr>
  <tr>
    <td>Gorra de entrenador</td>
    <td>Pokémon</td>
    <td>2</td>
  </tr>
  <tr>
    <td>Tri-Fuerza</td>
    <td>The Legend of Zelda</td>
    <td>2</td>
  </tr>
  <tr>
    <td>Máscara de Majora</td>
    <td>The Legend of Zelda</td>
    <td>2</td>
  </tr>
  <tr>
    <td>Pico</td>
    <td>Minecraft</td>
    <td>2</td>
  </tr>
  <tr>
    <td>Diamante</td>
    <td>Minecraft</td>
    <td>2</td>
  </tr>
</table>

<div id="Puntosdeinteraccion"></ol>


## **2.3. Puntos de interacción**

A continuación se concretan los diferentes puntos de elaboración de props:

* **Texturizador:** otorga las texturas necesarias al prop, por lo que es bastante utilizado. Este punto de interacción será el más crítico debido al número de assets que deben introducirse aquí y por ello habrá varias unidades en el juego.

* **Fundición de metales:** utilizado para crear todos aquellos objetos metálicos, que necesitan un material especialmente resistente.

* **Caldero mágico:** utilizado para crear objetos mágicos o especiales.

Por otro lado, podemos apreciar los diferentes puntos de entrega para cada saga:

* **_Super Mario Bros:_** una tubería verde, típica de este videojuego.

* **_Pac-Man:_** un "comecocos" gigante, que engulle los props entregados.

* **_Portal:_** un portal naranja, a no se sabe muy bien dónde.

* **_Pokémon:_** el clásico PC de Bill.

* **_The Legend of Zelda:_** *el típico cofre visto en la saga.* 

* **_Minecraft:_** una mesa de crafting.

<div id="FlujodejuegoGameLoop"></ol>


## **2.4. Flujo de juego (Game Loop)**

*Nota: las siguientes imágenes son esquemáticas e ilustrativas, y su perspectiva y disposición visual no se corresponde necesariamente con el aspecto real.*

El Game Loop estándar de una partida consistirá en lo siguiente:

* Al comenzar la partida, **se visualizará una sola vez el libro de recetas,** para que el jugador pueda apreciar en un solo vistazo qué props deberá elaborar y cómo es el proceso. Si el jugador se toma la molestia de memorizar los pasos, será recompensado al resultar más fácil la partida.

* Una vez cerrado el libro de recetas, comienza la partida como tal. Continuamente **van llegando props por la cinta transportadora** del escenario. **Si alguno de ellos no es recogido por el jugador,** sino que llega a la papelera situada al final de la cinta, **el jugador perderá una vida.**

![image alt text](assets/GDD/image_0.png)

![image alt text](assets/GDD/image_1.png)

* La mecánica principal consistirá en que el jugador vaya a la cinta a recoger un prop, y de ahí lo lleve al **punto de preparación** que corresponda según su receta. Una vez se esté preparando, deberá estar atento de **no dejarlo ahí demasiado tiempo, pues el punto se sobrecargará,** quedando inutilizado unos instantes y perdiendo el prop.

![image alt text](assets/GDD/image_2.png)

![image alt text](assets/GDD/image_3.png)

![image alt text](assets/GDD/image_4.png)

![image alt text](assets/GDD/image_5.png)

![image alt text](assets/GDD/image_6.png)

* Si todos los puntos de preparación están ocupados, pero siguen apareciendo props por la cinta, para evitar perderlos el jugador podrá recogerlos para **depositarlos en las mesas vacías del nivel** (que aunque abundantes, son limitadas, por lo que tampoco puede depositar absolutamente todos los props y detener así el ritmo de juego).

![image alt text](assets/GDD/image_7.png)

![image alt text](assets/GDD/image_8.png)

* Una vez se haya completado el proceso de preparación de un prop, pasando por todos los puntos necesarios, se deberá recoger y llevar al **punto de entrega de la saga a la que pertenezca.** Si se deposita en un punto de entrega erróneo, éste quedará inutilizado unos instantes, y se perderá el prop.

![image alt text](assets/GDD/image_9.png)

![image alt text](assets/GDD/image_10.png)

![image alt text](assets/GDD/image_11.png)

* En caso de que el jugador no recuerde o no sea capaz de deducir correctamente cuál es el proceso de preparación de un prop, es decir, su receta, **podrá consultar el libro visto al empezar la partida en algún punto del escenario,** aunque ir hasta allí, y visualizarlo, implica perder tiempo mientras aparecen props por la cinta, incentivando así al jugador a intentar memorizar o intuir las recetas en lugar de consultarlas continuamente.

* Una vez se hayan **entregado el número de props necesario, se superará el nivel satisfactoriamente,** pudiendo visualizar la puntuación obtenida y pasando al siguiente nivel, si procede. Si, por el contrario, no es capaz de elaborar el número de props requerido, sino que en su lugar **pierde todas las vidas de las que dispone, perderá la partida,** teniendo que reiniciar el nivel.

<div id="Camara"></ol>


## **2.5. Cámara**

La cámara del juego será **fija o estática, con una perspectiva aérea,** casi cenital, de forma que permita visualizar la totalidad del nivel en pantalla, al mismo tiempo que se aprecia la profundidad y el 3D del juego.

<div id="Controles"></ol>


## **2.6. Controles**

Los controles consistirán únicamente del **click o pulsación sobre la región de la pantalla, propios del tipo "Point & Click".** Por lo tanto, pulsar sobre un lugar, el personaje se desplazará hacia dicho punto. Si pulsamos sobre un objeto, el personaje se desplazará hasta él, y posteriormente lo recogerá o realizará la acción correspondiente. Para mostrar visualmente qué es lo que va a hacer el personaje, **se mostrará un pequeño indicador visual sobre el punto** del mapa o el objeto determinado.

Cabe mencionar que, si mientras se está desplazando o realizando una acción, recibe otra orden (hacemos click en otro punto de la pantalla), **se anulará la acción anterior, dando prioridad a la nueva.**

<div id="Niveles"></ol>


## **2.7. Niveles**

En el juego final, podremos diferenciar **dos niveles completos distintos,** con props y distribución espacial diferentes.

Un ejemplo de diseño de nivel esquemático sería el siguiente:

![image alt text](assets/GDD/image_12.png)

Los mapas tendrán un número de **puntos de preparación ("hornos")** distintos repartidos por el escenario, a la vez que una serie **puntos de entrega (“tuberías”),** que serán referentes a su respectivo juego. 

El jugador podrá llevar los props que **salgan de la cinta transportadora a una de las mesas o bien a los "hornos", y una vez preparados, a las “tuberías”.**

En todo momento podrá consultar el **libro de recetas o combinaciones,** colocado en un lugar generalmente alejado de la acción.

<div id="inInterfaz"></ol>

# **3. Interfaz**

<div id="DiagramadeFlujo"></ol>


## **3.1. Diagrama de Flujo**

A continuación, se muestra el diagrama de flujo y transición entre pantallas durante el desarrollo del juego:

<div id="PantalladeNivel"></ol>


## **3.2. Pantalla de Nivel (HUD)**

En las siguientes imágenes vemos un esbozo inicial y esquemático de los elementos que constituyen el HUD del juego (su distribución, forma y color pueden no coincidir necesariamente con los de la versión final, se trata simplemente de una aproximación al concepto):

 ![image alt text](assets/GDD/image_13.png)

Como vemos, se visualiza el progreso en el nivel (los props entregados sobre el total necesario), y las vidas restantes del jugador.

La idea fundamental detrás del diseño de interfaces del juego es minimizar el número de elementos y menús, para que sea lo más usable posible en pantallas de dispositivos móviles.

<div id="arteysonido"></ol>

# **4. Arte y Sonido**

<div id="Arte"></ol>

## **4.1. Arte**

El apartado artístico del videojuego será colorido y alegre, animaciones algo exageradas y, en general, una estética de 3D *cartoon*.

El arte final se irá incorporando conforme se tengan los recursos necesarios.

<div id="Sonido"></ol>

## **4.2. Sonido**

Los sonidos del juego deberán acompañar a su estilo visual, de forma que deberán tener un toque alegre, divertido y amigable.

El sonido se irá incorporando conforme se tengan los recursos necesarios.

<div id="roles"></ol>


# **5. Roles**

Para la realización del trabajo tendremos un equipo de 6 personas, siguiendo los siguientes roles:

* *Alejandro Camuñas Casas*: Game Designer, Rigger y Animador 3D.

* *Andrés Felipe García:* Modelador 3D y Desarrollador Web.

* *Guillermo Amigó Urda:* Game Designer y Sound Designer.

* *Jose Daniel Campos Galán:* Programador.

* *Pablo Rodriguez Vicente:* Programador.

* *Raquel Gastón Vicente: *Artista 2D y Modeladora 3D.

