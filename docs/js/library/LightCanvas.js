class Vector2 {
  constructor(x, y) {
    this._x = x;
    this._y = y;
  }
  get x() {
    return this._x;
  }
  get y() {
    return this._y;
  }
  set x(x1) {
    this._x = x1;
  }
  set y(y1) {
    this._y = y1;
  }

  /**
   * @returns {number}
   * Return the module of the vector
   */
  get magnitude() {
    return Math.sqrt(this.x * this.x + this.y * this.y);
  }

  /**
   *
   * @param {Vector2} vector
   * @returns {Vector2} the addition
   */
  add(vector) {
    let nV = new Vector2(this.x + vector.x, this.y + vector.y);
    return nV;
  }

  /**
   *
   * @param {number} number
   * @returns {Vector2} the multiplication
   */
  mult(number) {
    let nV = new Vector2(this.x * number, this.y * number);
    return nV;
  }
  dot(vector) {
    return this.x * vector.x + this.y * vector.y;
  }

  distance(point) {
    let deltaX = this.x - point.x;
    let deltaY = this.y - point.y;
    let vector = new Vector2(deltaX, deltaY);
    return vector.magnitude;
  }

  normalize() {
    if (this.magnitude == 0) {
      return 0;
    } else {
      let magnitude = this.magnitude;
      this.x /= magnitude;
      this.y /= magnitude;
    }
  }
}

class AudioObject{


  constructor(audioSrc, loopBuffer){
    this._audioElement = new Audio(audioSrc);
    this._loopBuffer = loopBuffer;
    this._isLoad = false;
    this._audioElement.oncanplay = this.SetPlayTrue.bind(this);
    this._audioElement.load();
  }

  PlayOneShot(){
    if(this._isLoad){
    this._audioElement.loop = false;
    this._audioElement.play();
    }else {
      setTimeout(this.PlayOneShot.bind(this),10);
    }
  }

  PlayOnLoop(){
    this._audioElement.pause();
    if(this._isLoad){ 
      this._audioElement.addEventListener('timeupdate', this.Loop.bind(this), false);    
      //this._audioElement.loop = true;
      this._audioElement.play();
      }else {
        setTimeout(this.PlayOnLoop.bind(this),10);
      }
  }

  Loop(){
    var buffer = this._loopBuffer;
        if(this._audioElement.currentTime > this._audioElement.duration - buffer){
            this._audioElement.currentTime = 0
            this._audioElement.play()
        }
  }

  Stop(){
    this._audioElement.pause();
    this._audioElement.currentTime = 0.0;
  }

  SetPlayTrue(ev){
    this._isLoad = true;
    this._audioElement.pau
  }

  ChangeAudioSrc (src){
    this._audioElement.src = src;
    this._isLoad = false;
    this._audioElement.load();
  }

  set volume (volumeAmount){
    this._audioElement.volume = volumeAmount;
  }
  get volume(){return this._audioElement.volume;}

}


class Animation {
  constructor(imgSrc, frames, wFrame, hFrame, time, initialFrame) {
    this._image = new Image(wFrame * frames, hFrame * frames);
    this._image.onload = function(){loadingCount++;}
    this._image.src = imgSrc;
    this._frames = frames;
    this._wFrame = wFrame;
    this._hFrame = hFrame;
    this._actualFrame = 0;
    this._timeFrames = time;
    this.incrementalTime = 0;
    this._initialFrame = initialFrame
  }

  /**
   * @param {CanvasRenderingContext2D} renderCanvas
   */
  RenderFrame(renderCanvas, position) {
    renderCanvas.drawImage(
      this._image,
      this._initialFrame + (this._wFrame * this.actualFrame),
      0,
      this._wFrame,
      this._hFrame,
      position.x,
      position.y,
      this._wFrame,
      this._hFrame
    );
  }

  UpdateFrame(deltaTime) {
    this.incrementalTime += deltaTime;
    if (this._timeFrames <= this.incrementalTime) {
      this.incrementalTime = 0;
      this.actualFrame = ++this.actualFrame % this._frames;
    }
  }
  get actualFrame() {
    return this._actualFrame;
  }
  set actualFrame(frame) {
    this._actualFrame = frame;
  }
}
class SpriteObject {
  /**
     * 
     * @param {String} name 
     * @param {Vector2} position 
     * @param {String} imgSrc 
     * @param {number} height 
     * @param {number} width      
     
         
     */
  constructor(name, position, imgSrc, height, width) {
    this._position = position;
    this._anchor = new Vector2(0,0);
    this.name = name;
    this.sprite = new Image(width, height);
    this.sprite.src = imgSrc;
    this._active = true;
    this.idleSprite = this.sprite;
    this._height = height;
    this._width = width;

    this._animationList = new Map();
    this._actualAnim = null;
    this._velocity = new Vector2(0, 0);
  }
  /**
   * @returns {Boolean}
   */
  get active() {
    return this._active;
  }
  /**
   * @param {Boolean} active
   */
  set active(active) {
    this._active = active;
  }

  /**
   * @returns {number}
   */
  get height() {
    return this._height;
  }
  /**
   * @returns {number}
   */
  get width() {
    return this._width;
  }
  /**
   * @returns {Vector2}
   */
  get position() {
    return this._position;
  }

  set height(h) {
    this._height = h;
  }

  set width(w) {
    this._width = w;
  }
  set position(pos) {
    this._position = pos;
  }

  /**
   * @param {Vector2} v
   */
  set anchor(v){
    this._anchor.x = v.x;
    this._anchor.y = v.y;
    this._position.x = this._position.x - this._anchor.x*this._width;
    this._position.y = this._position.y - this._anchor.y*this._height;
  }

  set velocity(vector) {
    this._velocity = vector;
  }

  /**
   * @returns {Vector2}
   */
  get velocity() {
    return this._velocity;
  }
  /**
   * @returns {Animation}
   */
  get actualAnim() {
    return this._actualAnim;
  }
  set actualAnim(anim) {
    this._actualAnim = anim;
  }
  /**
   * @param {CanvasRenderingContext2D} renderCanvas
   */
  Render(renderCanvas) {
    let xAbsoluteRight = this.position.x + this.width*(1-this._anchor.x);
    let yAbsoluteDown = this.position.y + this.height*(1-this._anchor.y);
    let yAbsoluteUp = this.position.y - this._height*(this._anchor.y);
    let xAbsoluteLeft = this.position.x - this.width*this._anchor.x;


    //We only render if our Object is at the screen


    if((xAbsoluteRight < 0 || xAbsoluteLeft > 1280) || (yAbsoluteDown < 0 || yAbsoluteUp>720)){}
    else{
    if (this.actualAnim == null)
      renderCanvas.drawImage(
        this.sprite,
        this.position.x,
        this.position.y,
        this.width,
        this.height
      );
    else this.actualAnim.RenderFrame(renderCanvas, this.position);
    }
  }

  /**
   *
   * @param {number} timeDelta
   * @param {CanvasRenderingContext2D} hitbox
   */
  Update(timeDelta, hitbox) {
    let deltaPos = this.velocity.mult(timeDelta);
    this.position = this.position.add(deltaPos);
    if (this.actualAnim != null) {
      this.actualAnim.UpdateFrame(timeDelta);
    }
  }

  AddAnimation(animation, name) {
    this._animationList.set(name, animation);
  }

  SetAnimation(name) {
    this.actualAnim = this._animationList.get(name);
  }
}
var _actualScene;

class HitableObject extends SpriteObject {
  /**
       * 
       * @param {String} name 
       * @param {Vector2} position 
       * @param {String} imgSrc 
       * @param {number} height 
       * @param {number} width 
       
       */
  constructor(name, position, imgSrc, height, width) {
    super(name, position, imgSrc, height, width);
    this._hitColor = "#ffffff";
    this._activeHit = true;
  }
  
  /**
   * @returns {String}
   */
  get hitColor() {
    return this._hitColor;
  }
  /**
   * @param {String}
   */
  set hitColor(color) {
    this._hitColor = color;
  }

  activate() {
    this._activeHit = true;
  }

  deactivate() {
    this._activeHit = false;
  }

  
  /**
   *
   * @param {Object} eventInfo the click info
   */
  OnClick(eventInfo) {
      console.log(this.name);
  }
  
  /**
       
  * @param {number} timeDelta 
  * @param {CanvasRenderingContext2D} hitbox 
  */
  Update(timeDelta, hitbox) {
    super.Update(timeDelta, hitbox);
    if (this._activeHit) {
      hitbox.fillStyle = this.hitColor;
      hitbox.fillRect(
        this.position.x,
        this.position.y,
        this.width,
        this.height
      );
    }
  }
}
  

class TextObject {
  /**
   *
   * @param {String} text
   * @param {Vector2} pos
   */
  constructor(text, pos) {
    this._position = pos;
    this._text = text;
  }

  /**
   * @returns {String}
   */
  get text() {
    return this._text;
  }

  set text(t) {
    this._text = t;
  }

  /**
   * @returns {Vector2}
   */
  get position() {
    return this._position;
  }
  set position(pos) {
    this._position = pos;
  }
  /**
   * @param {CanvasRenderingContext2D} renderCanvas
   */
  Render(renderCanvas) {
    renderCanvas.fillText(this.text);
  }

  Update() {}
}

class CanvasManager {
  /**
   *
   * @param {String} canvasName
   * @param {number} w
   * @param {number} h
   *
   */
  constructor(canvasName, w, h) {
    this.canvasElement = document.getElementById(canvasName);
    this._targetHeight = h;
    this._targetWidth = w;
    this.heightRelation = h / this.canvasElement.height;
    this.widhthRelation = w / this.canvasElement.width;
    this.canvasScene = this.canvasElement.getContext("2d");
    this.hitcanvas = document.getElementById(canvasName + "-hitbox");
    this.canvasElement.addEventListener("click", this.OnClick.bind(this));
    this.hitScene = this.hitcanvas.getContext("2d");
    CanvasManager.actualScene = this.canvasScene;
    this.objectList = [];
    for (let i = 0; i < 5; i++) {
      this.objectList[i] = [];
    }
    this.timePased = 0;
    this.clickObjects = new Map();
  }
  /**
   * @returns {number}
   */
  get targetHeight() {
    return this._targetHeight;
  }
  /**
   * @returns {number}
   */
  get targetWidth() {
    return this._targetWidth;
  }

  set targetHeight(h) {
    this._targetHeight = h;
  }
  set targetWidth(w) {
    this._targetWidth = w;
  }

  static get actualScene() {
    return _actualScene;
  }
  static set actualScene(scene) {
    _actualScene = scene;
  }

  /**
   *
   * @param {SpriteObject | HitableObject} object An Object to render
   * @param {number} layerMask The layer Mask
   */
  AddObject(object, layerMask) {
    if (object.Update != undefined && object.Render != undefined) {
      this.objectList[layerMask].push(object);
    }
    if (object.OnClick != undefined && object.hitColor != undefined) {
      let randomColor = this.getRamdomColor();
      object.hitColor = randomColor;
      this.clickObjects.set(randomColor, object);
    }
  }
  AddList(objectList) {
        for(let i = 0;i<objectList.length;i++){
            for(let obj of objectList[i]){
                this.AddObject(obj,i);
            }
        }
  }

  RenderAndUpdate(timeDelta) {
    this.canvasScene.clearRect(
      0,
      0,
      this.canvasElement.width,
      this.canvasElement.height
    );
    this.hitcanvas.width = this.hitcanvas.width;

    for (let objLayer of this.objectList) {
      for (let obj of objLayer) {
        if (obj.active) {

          obj.Render(this.canvasScene);


          obj.Update(timeDelta, this.hitScene);
        }
      }
    }
  }

  /**
   * Start the MainLoop
   */
  Start() {
    window.requestAnimationFrame(this.MainLoop.bind(this));
  }

  /**
   *
   * @param {number} timeStamp TimePased
   */
  MainLoop(timeStamp) {
    let timeDelta = (timeStamp - this.timePased) * 0.001;
    this.timePased = timeStamp;
    this.RenderAndUpdate(timeDelta);
    window.requestAnimationFrame(this.MainLoop.bind(this));
  }
  /**
   *
   * @param {MouseEvent} e
   */
  OnClick(e) {
    
    const mousePos = {
      y: e.clientY - this.hitcanvas.offsetParent.offsetTop + document.body.scrollTop,
      x: e.clientX - this.hitcanvas.offsetParent.offsetLeft + document.body.scrollLeft
    };

    let relationH = this.targetHeight / this.hitcanvas.offsetHeight;
    let relationW = this.targetWidth / this.hitcanvas.offsetWidth;

    mousePos.x *= relationW;
    mousePos.y *= relationH;

    const pixel = this.hitScene.getImageData(mousePos.x, mousePos.y, 1, 1).data;

    const color = `rgb(${pixel[0]},${pixel[1]},${pixel[2]})`;

    let clickedObject = this.clickObjects.get(color);

    if (clickedObject != null) {
      clickedObject.OnClick(mousePos);
    }
  }

  /**
   * @returns {String} the color
   */
  getRamdomColor() {
    const r = Math.round(Math.random() * 255);
    const g = Math.round(Math.random() * 255);
    const b = Math.round(Math.random() * 255);
    return `rgb(${r},${g},${b})`;
  }

  ClearCanvas() {
    for (let i = 0; i < 5; i++) {
      this.objectList[i] = [];
    }
  }
}
