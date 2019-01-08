mergeInto(LibraryManager.library,{

GoCredits : function () {
    $('html, body').animate({
        scrollTop: $("#team_section").offset().top + 20
    }, 1000);
},

SetSpanish : function (){
    ponerEspanol();
},

SetEnglish : function (){
    ponerIngles();
},

Resize : function(){
    var canvas = document.getElementById("#canvas");
    var w = canvas.width;
    var h = canvas.height;

    canvas.addEventListener("resize",function(){
        canvas.height = h;
        canvas.width = w;

    })
}

});