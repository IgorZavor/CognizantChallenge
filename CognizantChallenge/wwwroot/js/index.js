(function (){
    $('#btn-send').click(() => {
        $.ajax({
            url: '/CheckTask',
            type: 'POST',
            data: {
                userId: $('.name').attr('user-id'),
                script: $('.script').val(),
                language: $('.languages option:selected').attr('requested-name'),
                challengeId: $('.challenges option:selected').attr('challenge-id')
            },
            success: (response) => {
                const msg = response.message;
                if (!response.error){
                    alert(msg);
                    $('.challenges option:selected').remove();
                    setDescription();
                    if (!$('.challenges option').length){
                        $('.btn-send').attr('disabled');
                        alert('You have done all tasks!!');
                    }
                }
                else{
                    alert(msg);
                }
            },
            fail: () => {}
        });
        return false;
    });

    $('.challenges').change(() => {
        const challenges = getFromStorage('Challenges');
        const id = parseInt($('.challenges option:selected').attr('challenge-id'));
        $('.description').text(challenges.find(ch => ch.id === id).description);
    });
    
    $('.languages').change((el) => {
        const languages = getFromStorage('Languages');
        const id = parseInt($('.languages option:selected').attr('language-id'));
        $('.script ').text(languages.find(l => l.id === id).template);
    })
    
    function createNewUser(){
        debugger
        let method = '/CreateUserAndGetData';
        let isDataFilled = !!(getFromStorage('Languages') && getFromStorage('Challenges'));
        if (isDataFilled) {
            method = '/CreateUser'
        }
        $.ajax({
            url: method,
            type: 'POST',
            dataType: 'json',
            data: {
                name: $(".name").val(),
            },
            success: (response) => {
                if (!isDataFilled) {
                    createUserAndGetDataSuccess(response);
                }
                else{
                    createUserSuccess(response.user)
                }
                setUser(response.user);
                changeCreateButtonState();
                $('.btn-send').removeAttr('disabled');
            },
            fail: () => {}
        });
    }
    
    function createUserAndGetDataSuccess(response){
        const challenges = response.challenges.map(ch => ({id: ch.id, name: ch.name, description: ch.description }));
        const languages = response.languages.map(l => ({ id:l.id, name: l.name, template: l.template, requestedName: l.requestedName }));
        setStorage(response.user, languages, challenges);
        fillChallengesOptions();
        fillLanguagesOptions();
        setDescription();
    }
    
    function createUserSuccess(user){
        setItemToStorage('User', user);
    }
    
    
    function setItemToStorage(key, item){
        const sessionStorage = window.sessionStorage;
        sessionStorage.setItem(key, JSON.stringify(item));
    }
    
    function setStorage(user, languages, challenges){
        const sessionStorage = window.sessionStorage;
        sessionStorage.setItem('User', JSON.stringify(user));
        sessionStorage.setItem('Languages', JSON.stringify(languages));
        sessionStorage.setItem('Challenges', JSON.stringify(challenges));
    }
    
    function removeItemFromStorage(key){
        const sessionStorage = window.sessionStorage;
        sessionStorage.removeItem(key);
    }
    
    function getFromStorage(key){
        const sessionStorage = window.sessionStorage;
        const item = sessionStorage.getItem(key);
        return item && JSON.parse(item) || null;
    }

    function createAnotherUser() {
        removeItemFromStorage('User');
        $('.user-part ').addClass('hidden');
        $('.name').val('');
        changeCreateButtonState();
    }
    
    function fillChallengesOptions() {
        const challenges = JSON.parse(sessionStorage.getItem('Challenges'));
        challenges.forEach(challenge  => {
            $('.challenges').append(`<option challenge-id='${challenge.id}'>${challenge.name}</option>`);
        })
        
    }
    
    function setDescription (){
        const challenges = JSON.parse(sessionStorage.getItem('Challenges'));
        const selectedId = parseInt($('.challenges option:selected').attr('challenge-id'));
        $('.description').text(selectedId && challenges && challenges.length && challenges.find(ch => ch.id === selectedId).description || '');
    }
    
    function fillLanguagesOptions() {
        const languages = JSON.parse(sessionStorage.getItem('Languages'));
        languages.forEach(l => {
            $('.languages').append(`<option language-id=${l.id} requested-name='${l.requestedName}'>${l.name}</option>`)    
        })
        $('.script').text(languages[0].template);
    }
    
    function setUser(user){
        $('.name').attr('user-id', user.id).val(user.name);
        $('.user-part ').removeClass('hidden');
    }
    
    function changeCreateButtonState(){
        const user = getFromStorage('User');
        if (user && user.id){
            $('#btn-create-user').unbind('click').text('Create Another').click(createAnotherUser)   
        }
        else {
            $('#btn-create-user').unbind('click').text('Create').click(createNewUser)
        }
    }
    
    const user = getFromStorage('User')
    if (user){
        setUser(JSON.parse(window.sessionStorage.getItem('User')));
        fillLanguagesOptions()
        fillChallengesOptions();
        setDescription();
    }
    changeCreateButtonState();
})()