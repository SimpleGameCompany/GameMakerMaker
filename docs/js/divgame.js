var resizecolumms = function(ev){

    document.getElementById("img_border_game_01").style.height = document.getElementById('div_game').clientHeight + 'px';
    document.getElementById("img_border_game_01").style.width = document.getElementById('col_div_border_game_01').clientWidth + 'px';

    document.getElementById("img_border_game_02").style.height = document.getElementById('div_game').clientHeight + 'px';
    document.getElementById("img_border_game_02").style.width = document.getElementById('col_div_border_game_02').clientWidth + 'px';
}


window.addEventListener('resize',resizecolumms);
resizecolumms();


