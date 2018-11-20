
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
            audio.PlayOneShot();
            this.deactivate();
            canvasManager.clickObjects.delete(this.hitColor);
            //this.SetAnimation("clicked");
        }
    }

    Update(timeDelta, hitbox){
        super.Update(timeDelta, hitbox);
        this.OnCollision(timeDelta, hitbox);
    }

    OnCollision(timeDelta, hitbox){
        if(this._activeHit && this.position.x -(this._anchor.x*this.width) < this._player.position.x){
            this.CollisionEvent(timeDelta, hitbox);
        }
    }
    CollisionEvent(timeDelta, hitbox){
        this._active = false
        PauseGame(null);
    }

}

class Sewer extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits, open){
        super(name, position, imgSrc, height, width, player, hits);
        this._open = open;
        this._openAudio = new AudioObject("assets/audio/Alcantarilla/Alcantarilla_Open.ogg",0);
        this._closeAudio = new AudioObject("assets/audio/Alcantarilla/Alcantarilla_Close.ogg",0);
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
}

class FlyingObject extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this._deadAudio = new AudioObject("assets/audio/Avion/Avion_Eliminatedogg.ogg",0);
        this._loopAudio = new AudioObject("assets/audio/Avion/Avion_Loop.ogg",0.5);
        this._spawnAudio = new AudioObject("assets/audio/Avion/Avion_Spawn.ogg",0);
    }
    OnClick(e){
        //this._deadAudio.PlayOneShot();
        super.OnClick(e,this._deadAudio);
    }

    Update(timeDelta, hitbox){
        if(this.position.x-(this.width*this._anchor.x) <= 1280 && !this._inCanvas){
            this._spawnAudio.PlayOneShot();
            this._loopAudio.PlayOnLoop();
            this._inCanvas = true;
        }
            super.Update(timeDelta,hitbox);
        
    }
}

class Poop extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this._deadAudio = new AudioObject("assets/audio/Caca/Caca_Eliminated.ogg",0);
        this._spawnAudio = new AudioObject("assets/audio/Caca/Caca_Spawn.ogg",0);
    }

    OnClick(e){
        super.OnClick(e,this._deadAudio);
    }

    Update(timeDelta, hitbox){
        if(this.position.x-(this.width*this._anchor.x) <= 1280 && !this._inCanvas){
            this._spawnAudio.PlayOneShot();
            this._inCanvas = true;
        }
            super.Update(timeDelta,hitbox);
        
    }
}

class Dove extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this._deadAudio = new AudioObject("assets/audio/Paloma/Paloma_Eliminated.ogg",0);
        this._spawnAudio = new AudioObject("assets/audio/Paloma/Paloma_Spawn.ogg",0);
    }

    OnClick(e){
        super.OnClick(e,this._deadAudio);
    }

    Update(timeDelta, hitbox){
        if(this.position.x-(this.width*this._anchor.x) <= 1280 && !this._inCanvas){
            this._spawnAudio.PlayOneShot();
            this._inCanvas = true;
        }
            super.Update(timeDelta,hitbox);
        
    }
}

class Car extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits){
        super(name, position, imgSrc, height, width, player, hits);
        this._loopAudio = new AudioObject("assets/audio/Coche/Coche_Move.ogg",0.9);
        this._claxonAudio = new AudioObject("assets/audio/Coche/Coche_Bocina.ogg",0);
        this._deadAudio = new AudioObject("assets/audio/Coche/Coche_Eliminated.ogg",0);
        this._inCanvas = false;
    }

    OnClick(e){
        this._loopAudio.Stop();
        super.OnClick(e,this._deadAudio);
    }

    Update(timeDelta, hitbox){
        if(this.position.x-(this.width*this._anchor.x) <= 1280 && !this._inCanvas){
            this._loopAudio.PlayOnLoop();
            this._inCanvas = true;
        }
            super.Update(timeDelta,hitbox);
        
    }
}

class Dog extends Obstacle{
    constructor(name, position, imgSrc, height, width, player, hits, barkNum){
        super(name, position, imgSrc, height, width, player, hits);
        this._bark = barkNum;
        this._stopped = false;
        this.interval;
        this._dead = false;
        this._deadAudio = new AudioObject("assets/audio/Perro/Perro_Eliminated.ogg",0);
        this._warnAudio = new AudioObject("assets/audio/Perro/Perro_Warn.ogg",0);
        this._attackAudio = new AudioObject("assets/audio/Perro/Perro_Attack.ogg",0);
    }

    OnClick(e){
        super.OnClick(e);
        if(this._hits == 0){
            this._deadAudio.PlayOneShot();
            if(this.interval){
                clearInterval(this.interval);
            }
            this._dead = true;
        }
        
    }

    Update(timeDelta, hitbox){
        if(this._bark == 0 && !this._dead){
            clearInterval(this.interval);
            this._attackAudio.PlayOneShot();
            this.velocity = new Vector2(speed+65,0);
        }else if(this.position.x <= 1200 && !this._stopped && !this._dead){
            this.interval = setInterval(this.Ladrar.bind(this),1000);
            this._stopped = true;
        }
        super.Update(timeDelta, hitbox);
    }

    Ladrar() {
        this.SetAnimation("ladrar");
        this._warnAudio.PlayOneShot();
        this._bark--;
        console.log("Guau");
    }
}

class Scrollable{
    constructor(name, position, img, height, width, animName, frames, vel, fps){
        this.active = true;
        this.b1 = new SpriteObject(name, position, img, height, width);
        this.b1.velocity = new Vector2(vel,0);
        let position2 = position.add(new Vector2(width,0));
        this.b2 = new SpriteObject(name, position2, img, height, width);
        this.b2.velocity = new Vector2(vel,0);
        let anim = new Animation(animName,frames,width,height,1/fps,0);

        this.b1.AddAnimation(anim,"idle");
        this.b1.SetAnimation("idle");
        this.b2.AddAnimation(anim,"idle");
        this.b2.SetAnimation("idle");
    }

    Update(deltaTime, hitBox){
        this.b1.Update(deltaTime, hitBox);
        this.b2.Update(deltaTime, hitBox);

        if(this.b1.position.x <= -this.b1.width){
            this.b1.position.x = this.b2.position.x+this.b2.width;
        }else if(this.b2.position.x <= -this.b2.width){
            this.b2.position.x = this.b1.position.x+this.b1.width;
        }
    }

    Render(renderCanvas){
        this.b1.Render(renderCanvas);
        this.b2.Render(renderCanvas);
    }
}

var canvasManager;
var menuObjects;
var gameObjects;
var loading;
var loadingCount;
var totalLoading;
var imageCount;
window.addEventListener("load",function(ev){
    loading = $(".loading");
   
    canvasManager = new CanvasManager("gameCanvas",1280,720);
    canvasManager.ClearCanvas();
    StartLoad();
    LoadObjects(ev);
    LoadLevel("nivel1",gameObjects);
})
window.addEventListener("keydown",function(ev){
    if(ev.key=="e"){
        canvasManager.ClearCanvas();
    }else{
        canvasManager.AddList(gameObjects);
    }
})

document.addEventListener("dblclick",function(ev){
    ev.preventDefault();
})

var timmy; 
var speed = -50;
var transparencyPause;
var pauseContinue;
function LoadObjects(ev){
    loadingCount = 0;
    totalLoading = 0;
    gameObjects = [];
    for (let i = 0; i < 5; i++) {
        gameObjects[i] = [];
    }

    timmy = new SpriteObject("player", new Vector2(70,525),"none",205,138);
    let animation = new Animation("assets/img/Timmy_spritesheet.png",8,138,205,1/8,0);
    timmy.AddAnimation(animation,"idle");
    totalLoading++;
    timmy.SetAnimation("idle");
    timmy.anchor = new Vector2(0.5,0.5);
    gameObjects[2].push(timmy);

    let sun = new SpriteObject("sun",new Vector2(0,0),"none",720,1280);
    let sunAnim = new Animation("assets/img/Sol_spritesheet.png",8,1280,720,1/8,0);
    totalLoading++;
    sun.velocity = new Vector2(0,0);
    sun.AddAnimation(sunAnim,"idle");
    sun.SetAnimation("idle");
    gameObjects[0].push(sun);

    let clouds = new Scrollable("clouds",new Vector2(0,0),"none",720,2560,"assets/img/Nubes_spritesheet.png",4,speed/8,8);
    gameObjects[0].push(clouds);
    totalLoading++;
    let mountains = new Scrollable("mountains",new Vector2(0,0),"none",720,5120,"assets/img/Fondo_spritesheet.png",3,speed/2,8);
    gameObjects[0].push(mountains);
    totalLoading++;
    let sidewalks = new Scrollable("sidewalks",new Vector2(0,0),"none",720,1280,"assets/img/Aceras_spritesheet.png",4,speed,8);
    gameObjects[0].push(sidewalks);
    totalLoading++;
    let road = new Scrollable("road",new Vector2(0,0),"none",720,1280,"assets/img/Carretera_spritesheet.png",4,speed,8);
    gameObjects[0].push(road);
    totalLoading++;
    let opciones = new HitableObject("opciones",new Vector2(1280,0),"assets/img/opciones.png",50,50);
    opciones.anchor = new Vector2(1,0);
    opciones.OnClick = PauseGame;
    gameObjects[4].push(opciones);

    transparencyPause = new SpriteObject("transparencia",new Vector2(0,0),"assets/img/fondo.png",720,1280);
    pauseContinue = new HitableObject("continuar",new Vector2(640,300),"assets/img/continuar.jpg",200,350);
    pauseContinue.OnClick = function(ev){canvasManager.ClearCanvas();canvasManager.AddList(gameObjects)}
    pauseContinue.anchor = new Vector2(0.5,0.5);

}

function LoadLevel(jsonName,container){
    
    $.getJSON("assets/files/"+jsonName+".json", function (json) {
        for(var obj of json){
            switch(obj.type){
                case "dog":
                    let dog = new Dog(obj.name,new Vector2(obj.positionx,obj.positiony),"none",obj.height,obj.width,timmy,obj.hitnumber, obj.ladridos);
                    let dogRunning = new Animation("assets/img/PerroCorriendo_spritesheet.png",8,557,184,1/8,0);
                    totalLoading +=1;
                    dog.anchor = new Vector2(0.5,0.5);
                    dog.velocity = new Vector2(speed,0);
                    dog.AddAnimation(dogRunning,"run");
                    dog.SetAnimation("run");
                    container[obj.layer].push(dog);
                break;
                case "sewer":
                    let sewer = new Sewer(obj.name,new Vector2(obj.positionx,obj.positiony),"none",obj.height,obj.width,timmy,obj.hitnumber, obj.open);
                    let openSewer = new Animation("assets/img/Alcantarilla_spritesheet.png",4,105,39,1/8,0);
                    let closeSewer = new Animation("assets/img/Alcantarilla_spritesheet.png",4,105,39,1/8,105*4);
                    totalLoading +=2;
                    sewer.anchor = new Vector2(0.5,0.5);
                    sewer.velocity = new Vector2(speed,0);
                    sewer.AddAnimation(openSewer,"open");
                    sewer.AddAnimation(closeSewer,"close");
                    if(obj.open){
                        sewer.SetAnimation("open");
                    }else{
                        sewer.SetAnimation("close");
                    }
                    container[obj.layer].push(sewer);
                break;
                case "car":
                    let car = new Car(obj.name,new Vector2(obj.positionx,obj.positiony),"none",obj.height,obj.width,timmy,obj.hitnumber);
                    let carAnim = new Animation("assets/img/Coche_spritesheet.png",8,557,184,1/12,0);
                    totalLoading +=1;
                    car.anchor = new Vector2(0,0.5);
                    car.velocity = new Vector2(speed - 20,0);
                    car.AddAnimation(carAnim,"idle");
                    car.SetAnimation("idle");
                    container[obj.layer].push(car);
                break;
                case "dove":
                    let dove = new Dove(obj.name,new Vector2(obj.positionx,obj.positiony),"none",obj.height,obj.width,timmy,obj.hitnumber);
                    let doveAnim = new Animation("assets/img/Paloma_spritesheet.png",8,90,150,1/12,0);
                    totalLoading +=1;
                    dove.anchor = new Vector2(0.5,0.5);
                    dove.velocity = new Vector2(speed-20,0);
                    dove.AddAnimation(doveAnim,"idle");
                    dove.SetAnimation("idle");
                    container[obj.layer].push(dove);
                break;
                case "poop":
                    let poop = new Poop(obj.name,new Vector2(obj.positionx,obj.positiony),"none", obj.height,obj.width,timmy,obj.hitnumber);
                    let poopAnim = new Animation("assets/img/Caca_spritesheet.png",4,33,55,1/8,0);
                    totalLoading +=1;
                    poop.anchor = new Vector2(0.5,0.5);
                    poop.velocity = new Vector2(speed,0);
                    poop.AddAnimation(poopAnim,"idle");
                    poop.SetAnimation("idle");
                    container[obj.layer].push(poop);
                break;
                case "plane":
                    let plane = new FlyingObject(obj.name,new Vector2(obj.positionx,obj.positiony),"none",obj.height,onj.width,obj.hitnumber);
                    let planeAnim = new Animation("assets/img/Avion_spritesheet.png",4,1111,480,1/8,0);
                    totalLoading +=1;
                    plane.anchor = new Vector2(0.5,0.5);
                    plane.velocity = new Vector2(speed,0);
                    plane.AddAnimation(planeAnim,"idle");
                    plane.SetAnimation("idle");
                    container[obj.layer].push(plane);
                break; 
                default:
                break;
            }
        }
        StartGame(container,10);
    });
}

function StartGame(container,loadTime){
    canvasManager.ClearCanvas();
    canvasManager.AddList(container);
    canvasManager.RenderAndUpdate(0);
    setTimeout(function(){
        GoGame(loadTime);    
    },loadTime);
}

function GoGame (loadtime){
    if(loadingCount >= totalLoading){
        canvasManager.RenderAndUpdate(0);
        StopLoad();
        canvasManager.Start();
        
    }else{
        setTimeout(GoGame,loadtime,loadtime);
    }
}

function StartLoad(){
    loading.show();
}

function StopLoad(){
    loading.hide();
}


function PauseGame(ev){
    let fondo = new SpriteObject("fondo",new Vector2(0,0),canvasManager.canvasElement.toDataURL(),720,1280);
    
    canvasManager.ClearCanvas();
    canvasManager.AddObject(fondo,2);
    canvasManager.AddObject(transparencyPause,3);
    canvasManager.AddObject(pauseContinue,4);
    
}
