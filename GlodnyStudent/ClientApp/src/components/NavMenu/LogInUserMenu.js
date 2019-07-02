import React, { Component } from 'react'
import { Link } from 'react-router-dom';


export default class LogInUserMenu extends Component {
    render() {
        return (       
                <ul className="navMenuList" id="navMenuListLogin">
                    <li className="navMenuListItem"><Link className="navMenuListLink" to="/DodajRestauracje" >Dodaj restauracje</Link></li> 
                    <li className="navMenuListItem"><Link className="navMenuListLink" to="/ZmianaHasła" >Zmień hasło</Link></li>  
                    {(sessionStorage.getItem("role") === "Admin")?<li className="navMenuListItem navMenuListLink" onClick={(e)=>this.props.toggleAdminPanel(e)} >Panel Administracyjny</li>:null} 
                    <li className="navMenuListItem navMenuListLink" onClick={(e) =>this.props.handleLogOut(e)} >Wyloguj</li>
                </ul>
        )
    }
}
