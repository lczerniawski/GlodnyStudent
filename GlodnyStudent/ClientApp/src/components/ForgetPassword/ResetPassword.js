import React, { Component } from 'react'
import ForgetPassword from './ForgetPassword';
import NewResetPassword from './NewResetPassword';

export default class ResetPassword extends Component {

    checkPath(){
        const path = decodeURIComponent(window.location.pathname);
        const emaildRX = '^\/ResetHasła$';
        const  newPassordRx ="^\/ResetHasła\/[^\/]+\/[^\/]+$";
        if(path.match(emaildRX)){
            return <ForgetPassword/>;
        } else if(path.match(newPassordRx)){
            return <NewResetPassword/>
        }

    }



    render() {
        return (
            <div>
                {this.checkPath()}
            </div>
        )
    }
}
