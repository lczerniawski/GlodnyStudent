import React, { Component } from 'react'
import { Link } from 'react-router-dom';


export default class LogInUserMenu extends Component {
    render() {
        return ( 
            <div>
                <p className="userName">{sessionStorage.getItem("username")}</p>         
                <ul className="navList">
                    <li><Link to="/DodajRestauracje" >Dodaj restauracje</Link></li> 
                    <li><Link to="/ZmianaHasła" >Zmień hasło</Link></li>  
                    {(sessionStorage.getItem("role") === "Admin")?<li className="menuAdmin" onClick={(e)=>this.props.toggleAdminPanel(e)} >Panel Administracyjny</li>:null} 
                    <li className="logoutMenu" onClick={(e) =>this.props.handleLogOut(e)} >Wyloguj</li>
                </ul>
            </div>
        )
    }
}
