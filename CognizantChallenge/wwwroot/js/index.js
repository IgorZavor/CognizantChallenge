﻿(function (){
    $('#btn-send').click(() => {
        debugger;
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
                debugger;
                const msg = response.message;
                if (!response.error){
                    $('.challenges option:selected').remove();
                    const challenges = JSON.parse(sessionStorage.getItem('Challenges'));
                    setDescription(challenges);
                }

                alert(msg);
                
            },
            fail: () => {
                
            }
        });
        return false;
    });

    $('.challenges').change(() => {
        const id = parseInt($('.challenges option:selected').attr('challenge-id'));
        $('.description').text(challenges.find(ch => ch.id === id).description);
    });
    
    $('.languages').change((el) => {
        const id = parseInt($('.languages option:selected').attr('language-id'));
        $('.script ').text(languages.find(l => l.id === id).template);
    })
    
    function createNewUser(){
        $.ajax({
            url: '/CreateUser',
            type: 'POST',
            dataType: 'json',
            data: {
                name: $(".name").val(),
            },
            success: (response) => {
                const challenges = response.challenges.map(ch => ({id: ch.id, name: ch.name, description: ch.description }));
                const languages = response.languages.map(l => ({ id:l.id, name: l.name, template: l.template, requestedName: l.requestedName }));
                setStorage(response.user, languages, challenges);
                setStorage()
                setUser(response.user);
                fillChallengesOptions(challenges);
                fillLanguagesOptions(languages);
                setDescription(challenges);
                changeCreateButtonState();
            },
            fail: () => {

            }
        });
    }
    
    function setStorage(user, languages, challenges){
        const sessionStorage = window.sessionStorage;
        sessionStorage.setItem('User', JSON.stringify(user));
        sessionStorage.setItem('Languages', JSON.stringify(languages));
        sessionStorage.setItem('Challenges', JSON.stringify(challenges));
    }
    
    function clearStorage(){
        const sessionStorage = window.sessionStorage;
        sessionStorage.removeItem('User');
        sessionStorage.removeItem('Languages');
        sessionStorage.removeItem('Challenges');
    }

    function createNewAnotherUser() {
        clearStorage();
        $('.user-part ').addClass('hidden');
        $('#btn-create-user').val('Create').click(createNewUser);
    }
    
    function fillChallengesOptions(challenges) {
        challenges.forEach(challenge  => {
            $('.challenges').append(`<option challenge-id='${challenge.id}'>${challenge.name}</option>`);
        })
        
    }
    
    function setDescription (challenges){
        const selectedId = parseInt($('.challenges option:selected').attr('challenge-id'));
        if (selectedId) {
            $('.description').text(challenges.find(ch => ch.id === selectedId).description);
        }
    }
    
    function fillLanguagesOptions(languages) {
        languages.forEach(l => {
            $('.languages').append(`<option language-id=${l.id} requested-name='${l.requestedName}'>${l.name}</option>`)    
        })
        $('.script').text(languages[0].template);
    }
    
    function setUser(user){
        $('.name').attr('user-id', user.id).val(user.name);
        $('.user-part ').removeClass('hidden');
    }
    
    function changeCreateButtonState(user){
        if (user && user.id){
            $('#btn-create-user').val('Create Another').click(createNewAnotherUser)   
        }
        else {
            $('#btn-create-user').val('Create').click(createNewUser)
        }
    }
    
    const user = window.sessionStorage.getItem('User')
    if (window.sessionStorage.getItem('User')){
        setUser(JSON.parse(window.sessionStorage.getItem('User')));
        const languages = JSON.parse(sessionStorage.getItem('Languages'));
        fillLanguagesOptions(languages)
        const challenges = JSON.parse(sessionStorage.getItem('Challenges'));
        fillChallengesOptions(challenges);
        setDescription(challenges);
    }
    changeCreateButtonState(user);
})()


