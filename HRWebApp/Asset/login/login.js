

import { axios } from 'axios';
const email = document.querySelector('input[type=email]');
const password = document.querySelector('input[password]');
console.log(email, password);
const getLoginUser = async () => {
    let httpRequest = await axios(`https://localhost:44323/api/user/get?email={email}&password={password}`);
}
const btnLogin = document.querySelector('#btnSubmit');
btnLogin.addEventListener('submit', e => {
    e.preventDefault();
    //getLoginUser();
    console.log(e.target);

});

