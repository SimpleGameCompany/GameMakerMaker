var langu = "e";//e = English, s= Spanish
var html = document.getElementsByClassName("PDays");
var text = html[0].innerText;
var text2= html[1].innerText;

console.log("Entrando en language");


function lang() {//Inicializar...
    if (typeof (Storage) !== "undefined") {
        if (!localStorage.langu) {
            localStorage.langu = "e"; //Si no hay nada, por defecto English
        } else {
            langu = localStorage.langu;
        }

    } else {
        console.log("Languajes not supported");
    }
}

comprobarIdioma = function () {
    if (langu == "e") {
        console.log("Entrando en inglés");
        ponerIngles();
    } else if (langu == "s") {
        console.log("Entrando en español");
        ponerEspanol();
    }
}

actualizaDias = function () {
    
    
    var firstDate = new Date("09/19/2018");
    var EndDate = new Date();
    var timeDiff = Math.abs(firstDate.getTime() - EndDate.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    text = text.replace("%%", diffDays);
    text2 = text2.replace("%%", diffDays);
    for (var o of html) {
        if(langu == "e"){
            o.innerText = text;
        }else if(langu=="s"){
            o.innerText = text2;
        } 
        
        
    }



}

contarDias = function () {
    window.addEventListener("load",actualizaDias );
}



$("#buttonSpanish").click(function () {
    ponerEspanol();
    actualizaDias();

});
$("#buttonEnglish").click(function () {
    ponerIngles();
    actualizaDias();

});

function ponerEspanol() {
    $(".spanish").attr("hidden", false);
    $(".english").attr("hidden", true);


    if (typeof (Storage) !== "undefined") {

        localStorage.langu = "s";
        langu = "s";


    } else {
        console.log("Languajes not supported");
    }
}

function ponerIngles() {
    $(".english").attr("hidden", false);
    $(".spanish").attr("hidden", true);

    if (typeof (Storage) !== "undefined") {

        localStorage.langu = "e";
        langu = "e";


    } else {
        console.log("Languajes not supported");
    }

}
lang();

comprobarIdioma();
contarDias();
actualizaDias();