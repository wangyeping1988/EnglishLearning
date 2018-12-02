import axios from 'axios';

let base = 'http://localhost:15722/api';

export const requestLogin = params => { return axios.post(`${base}/Account/login`, params).then(res => res.data); };