
//#region variables
var timmy; 
var speed;
var distanciaRecorrida;
var tamaño;
var idioma = "_esp";
var mute = false;
var soundManager;
var canvasManager;
var menuObjects;
var gameObjects;
var loading;
var loadingCount;
var totalLoading;
var imageCount;
var actualLevel = 0;
var days = 0;
var levelname;
var lose = false;
var sky;
var cloud;
var hills;
var buildings;
var road;
var puntuacionText;
var fondoMenuPrincipal;
var fondoMenuScoresEsp;
var fondoMenuScoresEng;
var fondoMenuOptionsEsp;
var fondoMenuOptionsEng;
var fondoMenuInputName;
var transparencyPause;
var pauseContinue;
var pauseExit;
var jojoMensaje;
var scoreArray;
const storageName = "timmyScores";
var scoreParagraphs;
var inputText;
var daysText;
var scoreText;
var playerName;
const inglesInput= "Write your name";
const españolInput = "Escriba su nombre";
var parrafoInput;
var volume = 1;
var menuMusic;
var gameMusic;
var levelJson;
var recordText;
//#endregion

//#region objetos

class Input {
    constructor(pos,sampleText,title,fontsize){
        var onfocuss=    "this.value="+ '" '+ '"';
        this.inputHtml = $("<input type='text' onfocus='"+onfocuss + "'  name='"+title+"' value='"+sampleText+"' class = 'inputClass'></input>").appendTo($(canvasManager.canvasElement).parent());
        this.inputHtml.css("left",pos.x+"%");
        this.inputHtml.css("top",pos.y+"%");
        this.inputHtml.css("font-size",fontsize+"vw");
        
    }

    Show(){
        this.inputHtml.show();
    }
    Hide(){
        this.inputHtml.hide();
    }

    get text(){
       return this.inputHtml.val();
    }
}



class Obstacle extends HitableObject{
    /**
     * 
     * @param {String} name 
     * @param {Vector2} position 
     * @param {String} imgSrc 
     * @param {number} height 
     * @param {number} width 
     * @param {SpriteObject} player
     
     */
    constructor(name, position, imgSrc, height, width,player, hits){
        super(name, position, imgSrc, height, width);
        this._player = player;
        this._hits = hits; 
    }

    OnClick(e,audio){
        this._hits --;
        if(this._hits == 0){
            if(audio)
                audio.PlayOneShot();
            this.deactivate();
            canvasManager.clickObjects.delete(this.hitColor);
            setTimeout(function(){this.active = false;}.bind(this),5000);
            //this.active = false;
            //this.SetAnimation("clicked");
        }
    }

    Update(timeDelta, hitbox){
        super.Update(timeDelta, hitbox);
        this.OnCollision(timeDelta, hitbox);
    }

    OnCollision(timeDelta, hitbox){
        if(this._activeHit && this.position.x - this._player.width < this._player.position.x){
            this.CollisionEvent(timeDelta, hitbox);
      
        }
    }
    CollisionEvent(timeDelta, hitbox){
        this._activeHit = false
        
        LoseGame();
        //PauseGame(null);
    }

}

class SoundManager{
    constructor(){
        this.clickbutton1 = new AudioObject("assets/audio/Botones/Boton_1.ogg",0,1);
        this.clickbutton2 = new AudioObject("assets/audio/Botones/Boton_2.ogg",0,1);
    }

    PlayButton1(){
        if(!mute){
            this.clickbutton1.PlayOneShot();
        }
    }

    PlayButton2(){
        if(!mute){
            this.clickbutton2.PlayOneShot();
        }
    }

    StopAudio(){
        for(let i= 0; i<gameObjects.length; i++){
            for(let j = 0; j<gameObjects[i].length; j++){
                if(gameObjects[i][j].StopAudio){
                    gameObjects[i][j].StopAudio();
                }
            }
        }
    }
}



class Sewer extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits, open){
        super(name, position, imgSrc, height, width, player, hits);
        this._open = open;
        this._openAudio = new AudioObject("assets/audio/Alcantarilla/Alcantarilla_Open.ogg",0,volume);
        this._closeAudio = new AudioObject("assets/audio/Alcantarilla/Alcantarilla_Close.ogg",0,volume);
    }

    OnClick(e){
        if(this._open){
            this._open = !this._open;
            this._closeAudio.PlayOneShot();
            //canvasManager.clickObjects.delete(this.hitColor);
            this.SetAnimation("close");
        }else{
            this._open = !this._open;
            this._openAudio.PlayOneShot();
            //canvasManager.clickObjects.delete(this.hitColor);
            this.SetAnimation("open");
        }

    }

    CollisionEvent(timeDelta,hitbox){
        if(this._open){
            super.CollisionEvent();
        }
    }
    StopAudio(){}
}

class FlyingObject extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this._AvionDeadAudio = new AudioObject("assets/audio/Avion/Avion_Eliminated.ogg",0,volume);
        this._AvionLoopAudio = new AudioObject("assets/audio/Avion/Avion_Loop.ogg",0.5,volume);
        this._AvionSpawnAudio = new AudioObject("assets/audio/Avion/Avion_Spawn.ogg",0,volume);
    }

    OnClick(e){       
        if(this._hits == 1){
            this.StopAudio();
            this.velocity = new Vector2(300,-300/3); 
        }
        super.OnClick(e,this._AvionDeadAudio);
    }

    Update(timeDelta, hitbox){
        if(this.position.x <= 1280 && !this._inCanvas){
            this._AvionSpawnAudio.PlayOneShot();
            this._inCanvas = true;
            this.velocity = new Vector2(-300,300/3);
        }else if(this.position.x+this._width <= 1500 && !this._stopped){
            this._AvionLoopAudio.PlayOnLoop();
            setTimeout(this._AvionSpawnAudio.Stop.bind(this._spawnAudio),50);
            this._stopped = true;
            this.velocity = new Vector2(0,0);
        }
            super.Update(timeDelta,hitbox);
        
    }

    StopAudio(){
        this._AvionDeadAudio.Stop();
        this._AvionLoopAudio.Stop();
        this._AvionSpawnAudio.Stop();
    }
}

class Poop extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this._CacaDeadAudio = new AudioObject("assets/audio/Caca/Caca_Eliminated.ogg",0,volume);
        this._CacaSpawnAudio = new AudioObject("assets/audio/Caca/Caca_Spawn.ogg",0,volume);
    }

    OnClick(e){
        if(!this.clicked){
            this.clicked = true;
            this.SetAnimation("clicked");
            this._CacaDeadAudio.PlayOneShot();
            this.deactivate();
            canvasManager.clickObjects.delete(this.hitColor);
        }
    }

    Update(timeDelta, hitbox){
        if(this.position.x <= 1280 && !this._inCanvas){
            this._CacaSpawnAudio.PlayOneShot();
            this._inCanvas = true;
        }
            super.Update(timeDelta,hitbox);
        
    }

    StopAudio(){
        this._CacaDeadAudio.Stop();
        this._CacaSpawnAudio.Stop();
    }
}

class Dove extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this._PalomaDeadAudio = new AudioObject("assets/audio/Paloma/Paloma_Eliminated.ogg",0,volume);
        this._PalomaSpawnAudio = new AudioObject("assets/audio/Paloma/Paloma_Spawn.ogg",0,volume);
        this.dead = false;
    }

    OnClick(e){
        super.OnClick(e,this._PalomaDeadAudio);
        if(this._hits == 0){
            this.velocity = new Vector2(speed,-300);
            this.dead = true;
        }
    }

    Update(timeDelta, hitbox){
        if(this.position.x <= 1280 && !this._inCanvas){
            this._PalomaSpawnAudio.PlayOneShot();
            this._inCanvas = true;
        }
        if(this.position.x <= 400 && !this._shoot && !this.dead){
            let caca = new DovePoop("cacapaloma",new Vector2(this.position.x+25,this.position.y+75),"none",53,52,timmy,1);
            this._shoot = true;
        }
            super.Update(timeDelta,hitbox);
        
    }

    OnCollision(timeDelta, hitbox){}

    StopAudio(){
        this._PalomaDeadAudio.Stop();
        this._PalomaSpawnAudio.Stop();
    }
}

class DovePoop extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this.velocity = new Vector2(-110,190);
        let anim = new Animation("assets/img/CacaPaloma_spritesheet"+levelname+".png",4,52,53,1/8,0);
        this.AddAnimation(anim,"idle");
        this.SetAnimation("idle");
        canvasManager.AddObject(this,2);
    }

    OnClick(e){}
}

class Car extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this._CocheLoopAudio = new AudioObject("assets/audio/Coche/Coche_Move.ogg",0.9,volume);
        this._CocheClaxonAudio = new AudioObject("assets/audio/Coche/Coche_Bocina.ogg",0,volume);
        this._CocheDeadAudio = new AudioObject("assets/audio/Coche/Coche_Eliminated.ogg",0,volume);
        this._inCanvas = false;
    }

    OnClick(e){
        super.OnClick(e,this._CocheDeadAudio);
        if(this._hits==0){
            this._CocheLoopAudio.Stop();
            let x = Math.random();
            let y = Math.random()-0.5;
            this.velocity = new Vector2(x,y).normalize().mult(700);
            this.SetAnimation("die");
        }
    }

    Update(timeDelta, hitbox){
        if(this.position.x-(this.width*this._anchor.x) <= 1280 && !this._inCanvas){
            this._CocheLoopAudio.PlayOnLoop();
            this._inCanvas = true;
        }else if(this.position.x <= 400 && !this._near){
            this._CocheClaxonAudio.PlayOneShot();
            this._near = true;
        }
        super.Update(timeDelta,hitbox);
        
    }

    StopAudio(){
        this._CocheLoopAudio.Stop();
        this._CocheClaxonAudio.Stop();
        this._CocheDeadAudio.Stop();
    }
}

class Dog extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits, barkNum){
        super(name, position, imgSrc, height, width, player, hits);
        this._bark = barkNum;
        this._stopped = false;
        this.interval;
        this._dead = false;
        this._attacking = false;
        this._PerroDeadAudio = new AudioObject("assets/audio/Perro/Perro_Eliminated.ogg",0,volume);
        this._PerroBarkAudio = new AudioObject("assets/audio/Perro/Dog_bark_1.ogg",0,volume);
        this._PerroWarnAudio = new AudioObject("assets/audio/Perro/Perro_Warn.ogg",0,volume);
        this._PerroAttackAudio = new AudioObject("assets/audio/Perro/Perro_Attack.ogg",0,volume);
        
    }

    OnClick(e){
        if(this.interval){
            clearInterval(this.interval);
        }
        this._dead = true;
        this.StopAudio();
        let x = Math.random();
        let y = Math.random()-0.5;
        this.velocity = new Vector2(x,y).normalize().mult(700);
        this.SetAnimation("die");
        super.OnClick(e,this._PerroDeadAudio);    
    }

    Update(timeDelta, hitbox){
        if(this._bark == 0 && !this._attacking){
            this._PerroAttackAudio.PlayOneShot();
            this.SetAnimation("run");
            clearInterval(this.interval);
            this.velocity = new Vector2(speed-1000,0);
            this._attacking = true;
        }else if(this.position.x <= 1280 && !this._stopped){
            this._PerroWarnAudio.PlayOneShot();
            this.interval = setInterval(this.Ladrar.bind(this),1000);
            this._stopped = true;
        }
        if(lose){
            clearInterval(this.interval);
        }
        super.Update(timeDelta, hitbox);
    }

    Ladrar() {
        //this.SetAnimation("ladrar");
        if(!this._dead & !this._attacking){
            this._PerroBarkAudio.PlayOneShot();
        }
        this._bark--;
        console.log("Guau");
        if(this._bark == 0){
            clearInterval(this.interval);
        }

    }

    StopAudio(){
        this._PerroDeadAudio.Stop();
        this._PerroBarkAudio.Stop();
        this._PerroWarnAudio.Stop();
        this._PerroAttackAudio.Stop();
        clearInterval(this.interval);
    }
}

class Farola extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this.oscuridad = new SpriteObject("oscuridad", new Vector2(position.x-382,0),"assets/img/OscuridadFarola_nivel3.png",720,1274);
        this._FarolaSpawn = new AudioObject("assets/audio/Farola/Farola_Spawn.ogg",0,volume);
        this._FarolaEliminated = new AudioObject("assets/audio/Farola/Farola_Eliminated.ogg",0,volume);
        this.oscuridad.velocity = new Vector2(speed,0);
    }

    OnClick(e){
        super.OnClick(e,this._FarolaEliminated);    
        this.oscuridad.active = false;
    }

    OnCollision(){}

    Update(timeDelta, hitbox){
        if(this.oscuridad.position.x <= 1280 && !this._inCanvas){
            this._FarolaSpawn.PlayOneShot();
            this._inCanvas = true;
        }  
        super.Update(timeDelta, hitbox);
    }

    StopAudio(){
        this._FarolaSpawn.Stop();
        this._FarolaEliminated.Stop();
        clearInterval(this.interval);
    }
}

class Timmy extends SpriteObject{
    constructor(name, position,img,height,width){
        super(name,position,img,height,width); 
    }

    Update(deltaTime,hitBox){
        super.Update(deltaTime,hitBox);
        distanciaRecorrida -= deltaTime*speed;
        puntuacionText.puntos +=Math.floor(deltaTime*speed); 
        if(distanciaRecorrida >= tamaño){
            canvasManager.ClearCanvas();
            EndLevel();
        }
    }
}

class HTMLBackGround{
    constructor(name,img,vel,zIndex,scale){
        this.b1 =$("<img src ='"+img+"' class ='background'/>").appendTo( $(canvasManager.canvasElement).parent());
        this.b2 =$("<img src ='"+img+"' class ='background'/>").appendTo( $(canvasManager.canvasElement).parent());
        this.b2.click = function(ev){ev.preventDefault()}
        this.b1.click = function(ev){ev.preventDefault()}
        this.b1.css("zIndex",zIndex);
        this.b2.css("zIndex",zIndex);
        this.b1.css("width",100*scale +"%");
        this.b2.css("width",100*scale +"%");
        this.vel = vel;
        this.b1.pos = 0;
        this.b2.pos = 100*scale;
        this.scale = scale;
        this.active = true;
    }

    Update(deltaTime,hitBox){
        this.b1.pos +=(this.vel*deltaTime)
        this.b2.pos += (this.vel*deltaTime)
       
        if(this.b1.pos <=-100*this.scale){
            this.b1.pos = 100*this.scale-Math.abs(this.b2.pos);
        }
        if(this.b2.pos <=-100*this.scale){
            this.b2.pos = 100*this.scale-Math.abs(this.b1.pos);
        }


        this.b2.css("left",this.b2.pos + "%");
        this.b1.css("left",this.b1.pos + "%");
    }

    Hide(){
        this.b1.hide();
        this.b2.hide();
    }

    Show(){
        this.b1.show();
        this.b2.show();
    }

    Render(renderCanvas){}

    ChangeImg(imgSrc){
        this.b1.attr("src",imgSrc);
        this.b2.attr("src",imgSrc);
    }
    Reset(){
        this.b1.pos = 0;
        this.b2.pos = 100*this.scale;
    }
}
//#endregion

//#region menus
function StartMenuGame(){
    canvasManager.ClearCanvas();

        menuObjects = [];
        menuObjects[0] = [];
        menuObjects [1] = [];

        if (puntuacionText){
            puntuacionText.activate = false;
            puntuacionText.position = new Vector2(20,40);
        }

        if(sky)
            sky.Hide();

        if(hills)
            hills.Hide();

        if(road)
            road.Hide();

        if(buildings)
            buildings.Hide();

        if(cloud)
            cloud.Hide();

        if(fondoMenuPrincipal){
            fondoMenuPrincipal.Show();
        }else{
            fondoMenuPrincipal = new HTMLBackGround("menu","assets/img/Menu_principal.gif",0,1,1);
        }

        
        let start = new HitableObject("credits", new Vector2(640,411),"assets/img/Start_button.png",400,493);
        start.anchor = new Vector2(0.5,0.5);
        start.OnClick = function(ev){
            fondoMenuPrincipal.Hide();
            soundManager.PlayButton1();
            InputName();
        }

        let credits;
        if(idioma === "_esp")
            credits = new HitableObject("credits", new Vector2(840,490),"assets/img/Credits_button"+idioma+".png",84,273);
        else
            credits = new HitableObject("credits", new Vector2(872,490),"assets/img/Credits_button"+idioma+".png",84,241);
        credits.OnClick = function(ev){
            soundManager.PlayButton1();
            $('html, body').animate({
                scrollTop: $("#team_section").offset().top + 20
            }, 1000);
        }

        let opciones;
        if(idioma === "_esp")
            opciones = new HitableObject("opciones", new Vector2(150,480),"assets/img/Options_button"+idioma+".png",99,309);
        else
            opciones = new HitableObject("opciones", new Vector2(150,480),"assets/img/Options_button"+idioma+".png",99,262);
        opciones.OnClick = function(ev){
            soundManager.PlayButton1();
            fondoMenuPrincipal.Hide();
            OptionsMenu();
        }

        let puntuaciones;
        if(idioma === "_esp")
            puntuaciones = new HitableObject("opciones", new Vector2(439,570),"assets/img/Scores_button"+idioma+".png",77,403);
        else
            puntuaciones = new HitableObject("opciones", new Vector2(528,570),"assets/img/Scores_button"+idioma+".png",77,225);
        puntuaciones.OnClick = function(ev){
            soundManager.PlayButton1();
            fondoMenuPrincipal.Hide();
            PuntuacionesMenu();
        }

        menuObjects[1].push(start);
        menuObjects[1].push(credits);
        menuObjects[1].push(opciones);
        menuObjects[1].push(puntuaciones);
        
        canvasManager.AddList(menuObjects);
    
}

function OptionsMenu(){
    canvasManager.ClearCanvas();
    
        menuObjects = [];
        menuObjects[0] = [];
        menuObjects [1] = [];

        if(idioma === "_esp"){
            if(fondoMenuOptionsEsp){
                fondoMenuOptionsEsp.Show();
            }else{
                fondoMenuOptionsEsp = new HTMLBackGround("menu","assets/img/Menu_opciones"+idioma+".gif",0,1,1);
            }
        }else{
            if(fondoMenuOptionsEng){
                fondoMenuOptionsEng.Show();
            }else{
                fondoMenuOptionsEng = new HTMLBackGround("menu","assets/img/Menu_opciones"+idioma+".gif",0,1,1);
            }
        }

        let flecha;
        flecha = new HitableObject("continuar", new Vector2(130,570),"none",52,106);
        let flechaAnim;
        flechaAnim = new Animation("assets/img/Flecha_spritesheet.png",4,106,52,1/8,0);
        flecha.AddAnimation(flechaAnim,"idle");
        flecha.SetAnimation("idle");
        flecha.OnClick = function(ev){
            soundManager.PlayButton1();
            if(fondoMenuOptionsEsp)
                fondoMenuOptionsEsp.Hide();
            if(fondoMenuOptionsEng)
                fondoMenuOptionsEng.Hide();
            StartMenuGame();
        }

        let idiomaTexto;
        if(idioma === "_esp")
            idiomaTexto = new SpriteObject("idioma", new Vector2(233,472),"assets/img/Language"+idioma+".png",114,315);
        else
            idiomaTexto = new SpriteObject("idioma", new Vector2(233,472),"assets/img/Language"+idioma+".png",114,315);

        let ingles = new HitableObject("ingles", new Vector2(606,472),"none",101,162);
        let inglesAnim;
        if(idioma === "_esp"){
            inglesAnim = new Animation("assets/img/Bandera_Eng_spritesheet.png",4,162,101,1/8,0);
        }else{
            inglesAnim = new Animation("assets/img/Bandera_Eng_seleccionada_spritesheet.png",4,162,101,1/8,0);
        }
        ingles.AddAnimation(inglesAnim,"idle");
        ingles.SetAnimation("idle");
        ingles.OnClick = function(ev){
            soundManager.PlayButton1();
            if(idioma === "_esp"){
                if(fondoMenuOptionsEsp)
                    fondoMenuOptionsEsp.Hide();
                idioma = "_eng"
                OptionsMenu();
                ponerIngles();
                scoreText = "Score: ";
                if(days != null)
                days.textT = "Completed Days: ";
                pauseContinue = new HitableObject("continuar", new Vector2(640, 280), "assets/img/Pause_menu_resume" + idioma + ".png", 97, 509);
                pauseContinue.OnClick = function (ev) { canvasManager.ClearCanvas(); canvasManager.AddList(gameObjects) }
                pauseContinue.anchor = new Vector2(0.5, 0.5);

                pauseExit = new HitableObject("salir", new Vector2(640, 370), "assets/img/Pause_menu_exit" + idioma + ".png", 80, 163);
                pauseExit.OnClick = function (ev) { canvasManager.ClearCanvas(); soundManager.StopAudio(); LoseLevel() }
                pauseExit.anchor = new Vector2(0.5, 0.5);
                recordText.text = "New record!";
                actualizaDias();
            }
        }

        let español = new HitableObject("español", new Vector2(840,472),"none",101,162);
        let españolAnim;
        if(idioma === "_eng"){
            españolAnim = new Animation("assets/img/Bandera_Esp_spritesheet.png",4,162,101,1/8,0);
        }else{
            españolAnim = new Animation("assets/img/Bandera_Esp_seleccionada_spritesheet.png",4,162,101,1/8,0);
        }
        español.AddAnimation(españolAnim,"idle");
        español.SetAnimation("idle");
        español.OnClick = function(ev){
            if(idioma === "_eng"){
                soundManager.PlayButton1();
                if(fondoMenuOptionsEng)
                    fondoMenuOptionsEng.Hide();
                idioma = "_esp"
                OptionsMenu();
                ponerEspanol();
                scoreText = "Puntos: "
                if(days != null)
                days.textT = "Dias Completados: ";
                pauseContinue = new HitableObject("continuar", new Vector2(640, 280), "assets/img/Pause_menu_resume" + idioma + ".png", 97, 509);
                pauseContinue.OnClick = function (ev) { canvasManager.ClearCanvas(); canvasManager.AddList(gameObjects) }
                pauseContinue.anchor = new Vector2(0.5, 0.5);

                pauseExit = new HitableObject("salir", new Vector2(640, 370), "assets/img/Pause_menu_exit" + idioma + ".png", 80, 163);
                pauseExit.OnClick = function (ev) { canvasManager.ClearCanvas(); soundManager.StopAudio(); LoseLevel() }
                pauseExit.anchor = new Vector2(0.5, 0.5);
                recordText.text = "¡Nuevo récord!";
                actualizaDias();
            }
        }

        let sonido = new HitableObject("sonido", new Vector2(720,281),"none",91,101);
        let sonidoAnim;
        if(mute){
            sonidoAnim = new Animation("assets/img/VolumenOff_spritesheet.png",4,101,91,1/8,0);
        }else{
            sonidoAnim = new Animation("assets/img/VolumenOn_spritesheet.png",4,101,91,1/8,0);
        }
        sonido.AddAnimation(sonidoAnim,"idle");
        sonido.SetAnimation("idle");
        sonido.OnClick = function(){
            if(mute){
                mute = false;
                menuMusic.prop("muted",mute);
                gameMusic.prop("muted",mute);
                volume = 1;
                OptionsMenu();
            }else{
                mute = true;
                menuMusic.prop("muted",mute);
                gameMusic.prop("muted",mute);
                volume = 0;
                OptionsMenu();
            }
            soundManager.PlayButton1();
        }


        let sonidoText;
        if(mute){
            sonidoText = new SpriteObject("sonido", new Vector2(460,274),"assets/img/Sound"+idioma+".png",104,235);
        }else{
            sonidoText = new SpriteObject("sonido", new Vector2(460,274),"assets/img/Sound"+idioma+".png",104,235);
        }

        menuObjects[1].push(flecha);
        menuObjects[1].push(idiomaTexto);
        menuObjects[1].push(español);
        menuObjects[1].push(ingles);
        menuObjects[1].push(sonido);
        menuObjects[1].push(sonidoText);
        
        canvasManager.AddList(menuObjects);
}

function PuntuacionesMenu(){
    canvasManager.ClearCanvas();
    
        menuObjects = [];
        menuObjects[0] = [];
        if(idioma === "_esp"){
            if(fondoMenuScoresEsp){
                fondoMenuScoresEsp.Show();
            }else{
                fondoMenuScoresEsp = new HTMLBackGround("menu","assets/img/Menu_scores"+idioma+".gif",0,1,1);
            }
        }else{
            if(fondoMenuScoresEng){
                fondoMenuScoresEng.Show();
            }else{
                fondoMenuScoresEng = new HTMLBackGround("menu","assets/img/Menu_scores"+idioma+".gif",0,1,1);
            }
        }
        let flecha = new HitableObject("continuar", new Vector2(130,570),"assets/img/Flecha.png",52,106);
        let flechaAnim;
        flechaAnim = new Animation("assets/img/Flecha_spritesheet.png",4,106,52,1/8,0);
        flecha.AddAnimation(flechaAnim,"idle");
        flecha.SetAnimation("idle");
        flecha.OnClick = function(ev){
            soundManager.PlayButton1();
            if(fondoMenuScoresEsp)
                fondoMenuScoresEsp.Hide();
            if(fondoMenuScoresEng)
                fondoMenuScoresEng.Hide();
            scoreParagraphs.Hide();
            StartMenuGame();
        }
        menuObjects[0].push(flecha);
        
        if(scoreParagraphs == undefined){
            scoreParagraphs = [];
            for(let i = 0; i<10;i++){
                let parafo = new TextObject("",new Vector2(38,(i*5.75)+29),1.5,"Arial",canvasManager,"black");
                scoreParagraphs.push(parafo);
            }
            scoreParagraphs.Hide = function(){
                for(let i of this){
                    i.activate = false;
                    i.text = "";
                }
            }
            scoreParagraphs.Show = function(){
                for(let i of this){
                    i.activate = true;
                }
            }
            scoreParagraphs.FillText = function(scores){
                for(let i = 0;i<scores.length && i <10 ;i++){
                    scoreParagraphs[i].text = scores[i].name+" : "+ scores[i].puntos; 
                }
            }

        }
        scoreParagraphs.Show();
        scoreParagraphs.FillText(scoreArray);

        canvasManager.AddList(menuObjects);
}



function InputName(){
    canvasManager.ClearCanvas();
    if(fondoMenuInputName){
        fondoMenuInputName.Show();
    }else{
        fondoMenuInputName = new HTMLBackGround("input","assets/img/Menu_vacio.gif",0,1,1);
        fondoMenuInputName.Show();
    }
    if(idioma === "_esp"){
        parrafoInput = new SpriteObject("texto",new Vector2(327,117),"assets/img/Input_esp.png",75,640);
    }else{
        parrafoInput = new SpriteObject("texto",new Vector2(384,115),"assets/img/Input_eng.png",88,529);
    }

    inputText.Show();

    let continuar;
    if(idioma === "_esp")
        continuar = new HitableObject("continuar",new Vector2(640,550),"assets/img/Continue_input_button_esp.png",131,416);
    else
        continuar = new HitableObject("continuar",new Vector2(640,550),"assets/img/Continue_input_button_eng.png",131,416);
    continuar.anchor = new Vector2(0.5,0.5);
    
    continuar.OnClick = function(ev){
        let n = inputText.text.trim();
        if(n != ''){
            soundManager.PlayButton1();
            playerName = inputText.text
            inputText.Hide();
            fondoMenuInputName.Hide();
            parrafoInput.activate = false;
            menuMusic.StopAudio();
        
            loadGameFromLevel(ev);
        }else{
            soundManager.PlayButton2();
            InputName();
        }

    }

    let flecha = new HitableObject("continuar", new Vector2(130,570),"assets/img/Flecha.png",52,106);
    let flechaAnim;
        flechaAnim = new Animation("assets/img/Flecha_spritesheet.png",4,106,52,1/8,0);
        flecha.AddAnimation(flechaAnim,"idle");
        flecha.SetAnimation("idle");
    flecha.OnClick = function(ev){
        soundManager.PlayButton1();
        inputText.Hide();
        fondoMenuInputName.Hide();
        parrafoInput.activate = false;
        StartMenuGame();
    }
    canvasManager.AddObject(flecha,1);
    canvasManager.AddObject(continuar,0);
    canvasManager.AddObject(parrafoInput,1);

}
//#endregion

//#region cargas
function loadGameFromLevel(ev){
    StartLoad();
    canvasManager.ClearCanvas();
    if(idioma === "_esp"){
    puntuacionText = new TextObject("Puntos: ",new Vector2(0,0),3,"Arial",canvasManager,"white");
    
    daysText = new TextObject("Días completados: ", new Vector2(15,30),3,"Arial",canvasManager,"white");
    
    daysText.textT = "Días completados: ";
    daysText.activate = false;
    scoreText = " Puntos: ";
    }else{
        puntuacionText = new TextObject("Score: ",new Vector2(0,0),3,"Arial",canvasManager,"white");
        daysText = new TextObject("Completed days: ", new Vector2(15,30),3,"Arial",canvasManager,"white");
        daysText.textT = "Completed days: ";
        scoreText = " Score: " ;
        daysText.activate = false;
    }


    puntuacionText.textElement.addClass("scoreText");
    puntuacionText.activate=false;
    puntuacionText.puntos = 0;
    gameObjects = [];
    
    sky = new HTMLBackGround("sun","none",0,0,1);
    cloud = new HTMLBackGround("clouds","none",-50/8,1,2);
    hills = new HTMLBackGround("hills","none",-50/2,2,4);
    road = new HTMLBackGround("road","none",-50,4,4);
    buildings = new HTMLBackGround("build","none",-35,3,4);
    LoadLevel("nivelprueba",gameObjects);
}

function LoadLevel(jsonName,container){
    levelJson = jsonName;
    for (let i = 0; i < 6; i++) {
        gameObjects[i] = [];
    }

    puntuacionText.time = 0.5;
    puntuacionText.actualtime = 0;
    puntuacionText.Update = function(timeDelta,hitBox){
        if(this.actualtime >=this.time){
        puntuacionText.text = scoreText+Math.abs(puntuacionText.puntos);
        this.actualtime = 0;
        }else{
            this.actualtime+=timeDelta;
        }
    }
    $.getJSON("assets/files/"+jsonName+".json", function (json) {
        for(var obj of json){
            switch(obj.type){
                case "tutorialPoop":
                    let tutpoop;
                    if(idioma === "_esp")
                        tutpoop = new Poop("caca",new Vector2(obj.positionx-438/2,615-375+125),"none", 438,375,timmy,1);
                    else
                        tutpoop = new Poop("caca",new Vector2(obj.positionx-365/2,615-375+125),"none", 365,375,timmy,1);

                    let tutpoopAnim;
                    if(idioma === "_esp")
                        tutpoopAnim = new Animation("assets/img/Tutorial_spritesheet"+idioma+".png",4,438,375,1/8,0);
                    else
                        tutpoopAnim = new Animation("assets/img/Tutorial_spritesheet"+idioma+".png",4,438,375,1/8,0);
                    let tutpoopAnimDestroyed = new Animation("assets/img/WellDone_spritesheet"+idioma+".png",4,438,375,1/8,0);
                    totalLoading +=1;
                    //poop.anchor = new Vector2(0,0.5);
                    tutpoop.velocity = new Vector2(speed,0);
                    tutpoop.AddAnimation(tutpoopAnim,"idle");
                    tutpoop.AddAnimation(tutpoopAnimDestroyed,"clicked");
                    tutpoop.SetAnimation("idle");
                    container[4].push(tutpoop);
                break;
                case "speed":
                    speed = obj.speed + (days*-5);
                    LoadObjects(obj.nivel);
                break;
                case "tamaño":
                    tamaño = obj.valor;
                    distanciaRecorrida = 0;
                break;
                case "dog":
                    let dog = new Dog("perro",new Vector2(obj.positionx,500),"none",105,209,timmy, 1, 3);
                    let dogRunning = new Animation("assets/img/PerroCorriendo_spritesheet"+levelname+".png",8,209,105,1/16,0);
                    let dogAnim = new Animation("assets/img/PerroIdle_spritesheet"+levelname+".png",16,204,105,1/14,0);
                    let dogrotate = new Animation("assets/img/PerroRotando_spritesheet"+levelname+".png",8,205,105,1/16,0);
                    totalLoading +=1;
                    //dog.anchor = new Vector2(0,0.5);
                    dog.velocity = new Vector2(speed,0);
                    dog.AddAnimation(dogRunning,"run");
                    dog.AddAnimation(dogAnim,"idle");
                    dog.AddAnimation(dogrotate,"die");
                    dog.SetAnimation("idle");
                    container[3].push(dog);
                break;
                case "sewer":
                    let sewer = new Sewer("alcantarilla",new Vector2(obj.positionx,635),"none",39,105,timmy,1, obj.open);
                    let openSewer = new Animation("assets/img/Alcantarilla_spritesheet"+levelname+".png",4,105,39,1/8,0);
                    let closeSewer = new Animation("assets/img/Alcantarilla_spritesheet"+levelname+".png",4,105,39,1/8,105*4);
                    totalLoading +=2;
                    //sewer.anchor = new Vector2(0,0.5);
                    sewer.velocity = new Vector2(speed,0);
                    sewer.AddAnimation(openSewer,"open");
                    sewer.AddAnimation(closeSewer,"close");
                    if(obj.open){
                        sewer.SetAnimation("open");
                    }else{
                        sewer.SetAnimation("close");
                    }
                    container[1].push(sewer);
                break;
                case "car":
                    let car = new Car("coche",new Vector2(obj.positionx,460),"none",184,557,timmy,2);
                    let carAnim = new Animation("assets/img/Coche_spritesheet"+levelname+".png",8,557,184,1/12,0);
                    totalLoading +=1;
                    //car.anchor = new Vector2(0,0.5);
                    car.velocity = new Vector2(speed - 40,0);
                    let carrotate = new Animation("assets/img/CocheRotando_spritesheet"+levelname+".png",8,557,622,1/16,0);
                    car.AddAnimation(carAnim,"idle");
                    car.AddAnimation(carrotate,"die");
                    car.SetAnimation("idle");
                    container[4].push(car);
                break;
                case "dove":
                    let dove = new Dove("paloma",new Vector2(obj.positionx,obj.positiony),"none",150,90,timmy,1);
                    let doveAnim = new Animation("assets/img/Paloma_spritesheet"+levelname+".png",8,90,150,1/12,0);
                    totalLoading +=1;
                    //dove.anchor = new Vector2(0,0.5);
                    dove.velocity = new Vector2(speed-20,0);
                    dove.AddAnimation(doveAnim,"idle");
                    dove.SetAnimation("idle");
                    container[3].push(dove);
                break;
                case "poop":
                    let poop = new Poop("caca",new Vector2(obj.positionx,615),"none", 55,63,timmy,1);
                    let poopAnim = new Animation("assets/img/Caca_spritesheet"+levelname+".png",4,55,63,1/8,0);
                    let poopAnimDestroyed = new Animation("assets/img/BolsaCaca_spritesheet"+levelname+".png",4,55,63,1/8,0);
                    totalLoading +=1;
                    //poop.anchor = new Vector2(0,0.5);
                    poop.velocity = new Vector2(speed,0);
                    poop.AddAnimation(poopAnim,"idle");
                    poop.AddAnimation(poopAnimDestroyed,"clicked");
                    poop.SetAnimation("idle");
                    container[4].push(poop);
                break;
                case "plane":
                    let plane = new FlyingObject("avion",new Vector2(obj.positionx,-480),"none",480,1111,timmy,2);
                    let planeAnim = new Animation("assets/img/Avion_spritesheet"+levelname+".png",4,1112,480,1/8,0);
                    totalLoading +=1;
                    //plane.anchor = new Vector2(0,0.5);
                    plane.velocity = new Vector2(speed,0);
                    plane.AddAnimation(planeAnim,"idle");
                    plane.SetAnimation("idle");
                    container[4].push(plane);
                break; 
                case "farolas":
                    for(let i = 315+500; i<tamaño - 494; i+=obj.distancia){
                        let farola = new SpriteObject("farola", new Vector2(i-21,315),"none",269,43);
                        let farolaAnim = new Animation("assets/img/Farola_spritesheet"+levelname+".png",4,43,269,1/8,0);
                        farola.velocity = new Vector2(speed,0);
                        farola.AddAnimation(farolaAnim,"idle");
                        farola.SetAnimation("idle");
                        container[0].push(farola);
                        if(levelname === "_nivel3"){
                            let luz = new SpriteObject("farola", new Vector2(i-21-329,0),"assets/img/LuzFarola"+levelname+".png",698,698);
                            luz.velocity = new Vector2(speed,0);
                            container[0].push(luz);
                        }
                    }
                    for(var oscuridad of obj.oscuridades){
                        let farolaObs = new Farola("farolaobs",new Vector2(oscuridad+315-21-255,0),"assets/img/OscuridadHit.png",720,510,timmy,1);
                        farolaObs.velocity = new Vector2(speed,0);
                        container[5].push(farolaObs);
                        container[5].push(farolaObs.oscuridad);
                    }
                break;
                default:
                break;
            }
           
            //totalLoading+=14;
        }
        StartGame(container,1000);
    });
}

function LoadObjects(level){
    actualLevel = level;
    loadingCount = 0;
    totalLoading = 0;
   
    level = "_nivel"+level;
    levelname = level;
    
    timmy = new Timmy("player", new Vector2(110,449),"none",205,138);
    let animation = new Animation("assets/img/Timmy_spritesheet"+level+".png",8,138,205,1/8,0);
    timmy.AddAnimation(animation,"idle");
    totalLoading++;
    timmy.SetAnimation("idle");
    gameObjects[2].push(timmy);

    let black = new SpriteObject("black", new Vector2(tamaño,0),"assets/img/FondoFinal.png",720,1280);
    black.velocity = new Vector2(speed,0);
    gameObjects[4].push(black);
    let tunelLejos = new SpriteObject("tunellejos", new Vector2(tamaño-494,0),"none",720,496);
    let tunelLejosAnim = new Animation("assets/img/TunelLejos_spritesheet"+levelname+".png",4,496,720,1/8,0);
    tunelLejos.velocity = new Vector2(speed,0);
    tunelLejos.AddAnimation(tunelLejosAnim,"idle");
    tunelLejos.SetAnimation("idle");
    gameObjects[1].push(tunelLejos);
    let tunelCerca = new SpriteObject("tunelcerca", new Vector2(tamaño-495,0),"none",720,496);
    let tunelCercaAnim = new Animation("assets/img/TunelCerca_spritesheet"+levelname+".png",4,496,720,1/8,0);
    tunelCerca.velocity = new Vector2(speed,0);
    tunelCerca.AddAnimation(tunelCercaAnim,"idle");
    tunelCerca.SetAnimation("idle");
    gameObjects[4].push(tunelCerca);
    let tunelSalida = new SpriteObject("tunelsalida", new Vector2(0,0),"none",720,315);
    let tunelSalidaAnim = new Animation("assets/img/TunelSalida_spritesheet"+levelname+".png",4,315,720,1/8,0);
    tunelSalida.velocity = new Vector2(speed,0);
    tunelSalida.AddAnimation(tunelSalidaAnim,"idle");
    tunelSalida.SetAnimation("idle");
    gameObjects[4].push(tunelSalida); 

    sky.ChangeImg("assets/img/Cielo_animado"+level+".gif");  
    sky.vel = 0;
    gameObjects[0].push(sky);
    cloud.ChangeImg("assets/img/Nubes_animado"+level+".gif");
    cloud.vel = (speed*100)/1280/8;
    gameObjects[0].push(cloud);
    totalLoading++;
    buildings.vel = (speed*100)/1280/2;
    buildings.ChangeImg("assets/img/Edificios_animado"+level+".gif");
    hills.vel = (speed*100)/1280/4;
    hills.ChangeImg("assets/img/Fondo_animado"+level+".gif");
    road.vel = (speed*100)/1280;
    road.ChangeImg("assets/img/AceraConCarretera_animado"+level+".gif");
    gameObjects[0].push(buildings);
    gameObjects[0].push(hills);
    gameObjects[0].push(road);
    let opciones = new HitableObject("opciones",new Vector2(1196,18),"none",68,68);
    let opcionesAnim = new Animation("assets/img/Pause_button_spritesheet.png",4,68,68,1/8,0);
    opciones.AddAnimation(opcionesAnim,"idle");
    opciones.SetAnimation("idle");
    opciones.OnClick = PauseGame;
    gameObjects[5].push(opciones);

    

    gameObjects[5].push(puntuacionText);
}

function LoadButtonsAndBackGrounds(){
    transparencyPause = new SpriteObject("transparencia",new Vector2(0,0),"assets/img/fondo.png",720,1280);

    pauseContinue = new HitableObject("continuar",new Vector2(640,280),"assets/img/Pause_menu_resume"+idioma+".png",97,509);
    pauseContinue.OnClick = function(ev){canvasManager.ClearCanvas();canvasManager.AddList(gameObjects)}
    pauseContinue.anchor = new Vector2(0.5,0.5);

    pauseExit = new HitableObject("salir",new Vector2(640,370),"assets/img/Pause_menu_exit"+idioma+".png",80,163);
    pauseExit.OnClick = function(ev){canvasManager.ClearCanvas();soundManager.StopAudio();LoseLevel()}
    pauseExit.anchor = new Vector2(0.5,0.5);

    jojoMensaje = new SpriteObject("jojo",new Vector2(946,612),"none",57,310);
    let jojoAnim = new Animation("assets/img/To_be_continued_spritesheet.png",4,310,57,1/8,0);
    jojoMensaje.AddAnimation(jojoAnim,"idle");
    jojoMensaje.SetAnimation("idle");
}
//#endregion

//#region estados
function LoseGame(){
    //TODO
    lose = true;
    soundManager.StopAudio();
    canvasManager.RenderAndUpdate(0);
    let image = canvasManager.canvasScene.getImageData(0,0,1280,720);
    let d = image.data;
    for (var i=0; i<d.length; i+=4) {
        var r = d[i];
        var g = d[i+1];
        var b = d[i+2];
        d[i] = (r * .23) + (g *.61) + (b * .17);
        d[i+1] =  (r * .27) + (g *.62) + (b * .03);
        d[i+2] = (r * .18) + (g *.32) + (b * (-0.02));
      }
    canvasManager.canvasScene.putImageData(image,0,0);
    let fondo = new SpriteObject("fondo",new Vector2(0,0),canvasManager.canvasElement.toDataURL(),720,1280);
    /*let volverMenu = new HitableObject("volver",new Vector2(640,300),"assets/img/continuar.jpg",200,350);
    volverMenu.anchor = new Vector2(0.5,0.5);
    volverMenu.OnClick = function(ev){
        puntuacionText.puntos = 0;
        StartLoad();
        LoadLevel("nivel"+actualLevel,gameObjects);
    }*/
    canvasManager.ClearCanvas();
    for(let i = 0; i < 6; i++){
        gameObjects[i] = [];
    }
    sky.ChangeImg("assets/img/Cielo_sepia"+levelname+".png");
    hills.ChangeImg("assets/img/Fondo_sepia"+levelname+".png");
    road.ChangeImg("assets/img/AceraConCarretera_sepia"+levelname+".png");
    buildings.ChangeImg("assets/img/Edificios_sepia"+levelname+".png");
    cloud.ChangeImg("assets/img/Nubes_sepia"+levelname+".png");
    //TODO
    canvasManager.AddObject(fondo,0);
    //canvasManager.AddObject(volverMenu,5);
    canvasManager.AddObject(jojoMensaje,5);
    gameMusic.changeTrack("assets/audio/Music/Theme_Die.ogg");
    lose = false;
    setTimeout(LoseLevel,5000);
}

function EndLevel(){
    canvasManager.ClearCanvas();
    if (actualLevel == 3)
        days++;
    daysText.activate = true;
    daysText.text = daysText.textT+days;
    puntuacionText.position = new Vector2(14,40);
    let Continue
    if(idioma === "_esp")
        Continue = new HitableObject("continuar",new Vector2(850,460),"assets/img/Continue_button"+idioma+".png",71,306);
    else
        Continue = new HitableObject("continuar",new Vector2(864,460),"assets/img/Continue_button"+idioma+".png",71,278);

    let VolverAlMenu
    if(idioma === "_esp")
        VolverAlMenu = new HitableObject("continuar",new Vector2(814,550),"assets/img/MainMenu_button"+idioma+".png",71,378);
    else
        VolverAlMenu = new HitableObject("continuar",new Vector2(834,550),"assets/img/MainMenu_button"+idioma+".png",71,338);

    let Completado
    if(idioma === "_esp")
        Completado = new HitableObject("continuar",new Vector2(431,30),"assets/img/Level_completed"+idioma+".png",192,418);
    else
        Completado = new HitableObject("continuar",new Vector2(450,30),"assets/img/Level_completed"+idioma+".png",192,380);

    let fondoNegro = new SpriteObject("fondoNegro",new Vector2(0,0),"assets/img/FondoFinal.png",720,1280);

    Continue.OnClick = function(ev){
        soundManager.PlayButton1();
        canvasManager.ClearCanvas();
        StartLoad();
        let nextLevel = (actualLevel+1)
        if(nextLevel > 3){
            nextLevel = 1;
        }
        puntuacionText.position = new Vector2(0,0);
        daysText.activate = false;
        if(levelJson == "nivelprueba"){
            nextLevel--;
            puntuacionText.puntos = 0;
        }
        road.Reset();
        LoadLevel("nivel"+(nextLevel),gameObjects);
    }

    VolverAlMenu.OnClick = function(ev){
        soundManager.PlayButton1();
        canvasManager.ClearCanvas();      
        daysText.activate = false;
        WriteScore();
        gameMusic.StopAudio();
        menuMusic.trigger("play");
        StartMenuGame();
        puntuacionText.puntos = 0;
        days = 0;
    }

    canvasManager.AddObject(Continue,4);
    canvasManager.AddObject(VolverAlMenu,4);
    canvasManager.AddObject(Completado,4);
    canvasManager.AddObject(fondoNegro,1);
}

function LoseLevel(){
    canvasManager.ClearCanvas();
    gameMusic.trigger("pause");
    daysText.activate = true;
    daysText.text = daysText.textT+days;
    puntuacionText.position = new Vector2(14,40);
    if(scoreArray.length == 0 || Math.abs(puntuacionText.puntos) >  scoreArray[0].puntos){
        recordText.activate = true;
    }
    let Continue
    if(idioma === "_esp")
        Continue = new HitableObject("continuar",new Vector2(815,464),"assets/img/PlayAgain_button"+idioma+".png",67,375);
    else
        Continue = new HitableObject("continuar",new Vector2(831,454),"assets/img/PlayAgain_button"+idioma+".png",87,343);

    let VolverAlMenu
    if(idioma === "_esp")
        VolverAlMenu = new HitableObject("continuar",new Vector2(814,550),"assets/img/MainMenu_button"+idioma+".png",71,378);
    else
        VolverAlMenu = new HitableObject("continuar",new Vector2(834,550),"assets/img/MainMenu_button"+idioma+".png",71,338);

    let Completado
    if(idioma === "_esp")
        Completado = new HitableObject("continuar",new Vector2(527,30),"assets/img/GameOver"+idioma+".png",186,226);
    else
        Completado = new HitableObject("continuar",new Vector2(534,30),"assets/img/GameOver"+idioma+".png",192,212);

    let fondoNegro = new SpriteObject("fondoNegro",new Vector2(0,0),"assets/img/FondoFinal.png",720,1280);

    Continue.OnClick = function(ev){
        soundManager.PlayButton1();
        canvasManager.ClearCanvas();
        StartLoad();
        actualLevel = 1;
        puntuacionText.position = new Vector2(0,0);
        WriteScore();
        daysText.activate = false;
        puntuacionText.puntos = 0;
        days = 0;
        recordText.activate = false;
        road.Reset();
        LoadLevel("nivel1",gameObjects);
    }

    VolverAlMenu.OnClick = function(ev){
        soundManager.PlayButton1();
        canvasManager.ClearCanvas();
        WriteScore();
        gameMusic.StopAudio();
        menuMusic.trigger("play");
        daysText.activate = false;
        recordText.activate = false;
        puntuacionText.puntos = 0;
        days = 0;
        StartMenuGame();
       
    }

    canvasManager.AddObject(Continue,5);
    canvasManager.AddObject(VolverAlMenu,5);
    canvasManager.AddObject(Completado,5);
    canvasManager.AddObject(fondoNegro,0);
}

function StartGame(container,loadTime){
    canvasManager.ClearCanvas();
    setTimeout(function(){
        GoGame(loadTime);    
    },loadTime);
}

function GoGame (loadtime){   
        canvasManager.RenderAndUpdate(0);
        puntuacionText.activate=true;
        StopLoad();
        gameMusic.changeTrack("assets/audio/Music/Theme"+levelname+".mp3");
        canvasManager.AddList(gameObjects);
        canvasManager.Start();
}

function StartLoad(){
    loading.show();
}

function StopLoad(){
    loading.hide();
}

function PauseGame(ev){
    soundManager.PlayButton1();
    let fondo = new SpriteObject("fondo",new Vector2(0,0),canvasManager.canvasElement.toDataURL(),720,1280);
    
    canvasManager.ClearCanvas();
    canvasManager.AddObject(fondo,2);
    canvasManager.AddObject(transparencyPause,3);
    canvasManager.AddObject(pauseContinue,4);
    canvasManager.AddObject(pauseExit,4);
    
}


function WriteScore(){
    let name =playerName;//window.prompt("Su nombre","nombre");
    if(name != null && name != ""){
    let Score = {"puntos":Math.abs(puntuacionText.puntos),"name":name}
    scoreArray.push(Score);
    scoreArray.sort(compareNumbers);
    window.localStorage.setItem(storageName,JSON.stringify(scoreArray));
    }

}
//#endregion

//#region eventos
window.addEventListener("load",function(ev){
    console.log("debug");
    ponerEspanol();
    loading = $(".loading");
    loading.click = function(ev){ev.preventDefault()}
    loading.hide();
    canvasManager = new CanvasManager("gameCanvas",1280,720);
    soundManager = new SoundManager();
    LoadButtonsAndBackGrounds();
    StartMenuGame();
    scoreArray = window.localStorage.getItem(storageName);
    if(scoreArray == undefined){
        scoreArray = [];
        window.localStorage.setItem(storageName,JSON.stringify(scoreArray));
    }else{
        scoreArray = JSON.parse(scoreArray);
    }
    if(idioma !== "_esp")
        inputText = new Input(new Vector2(32,50),"name",inglesInput,2);
    else
        inputText = new Input(new Vector2(32,50),"name",españolInput,2);
    inputText.Hide();
    menuMusic = $("#music_menu");
    menuMusic.trigger("play");
    menuMusic.StopAudio = stopAudio.bind(menuMusic);
    gameMusic = $("#gameAudio");
    gameMusic.StopAudio = stopAudio.bind(gameMusic);
    gameMusic.prop("loop",true);
    gameMusic.changeTrack = function(src){
        this.StopAudio();
        this.attr("src",src);
        this.trigger("play");
    }
    if(idioma === "_esp")
        recordText = new TextObject("¡Nuevo récord!",new Vector2(15,50),3,"Arial",canvasManager,"#e15d5d");
    else
        recordText = new TextObject("New record!",new Vector2(15,50),3,"Arial",canvasManager,"#e15d5d");

    recordText.activate = false;
    
    canvasManager.Start(); 
})
function stopAudio(){
    //pause playing
    this.trigger('pause');
    //set play time to 0
    this.prop("currentTime",0);
  }
function compareNumbers(a, b) {
    return b.puntos - a.puntos;
  }
//#endregion