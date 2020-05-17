function numberOfAnswers() {
    const num = [].slice.call(document.querySelectorAll('.num'));

    for(let i = 0; i < num.length; i++) {
        if(num[i].innerText === '0') {
            num[i].style.backgroundColor = '#c0392b';
        }
    }
}

numberOfAnswers();

function showAnswer() {
    const questions = [].slice.call(document.querySelectorAll('.question-box'));

    questions.forEach(element => {
        element.addEventListener('click', function() {
            document.querySelector('.bg').classList.add('show');
            document.querySelector('.question-block').classList.add('show');
            
        });
    });

    document.querySelector('.close').addEventListener('click', function() {
        document.querySelector('.bg').classList.remove('show');
        document.querySelector('.question-block').classList.remove('show');
    });
}

showAnswer();