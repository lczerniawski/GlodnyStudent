import React, { Component } from 'react'
import { Link } from 'react-router-dom';

export default class GusetMenu extends Component {

    render() {
        return (    
            <ul className="navMenuList" > 

              <li className="navMenuListItem" >
                <form id="navMenuInputList" onSubmit={(e)=>this.props.handleLogIn(e)} >
                    <label  id="EmailLabel" className="NavMenuLabels" htmlFor="email">E-mail</label>
                    <input  id="email" className="navMenuListItemInput"  name="email" type="email" placeholder="E-mail" value={this.props.email}  onChange={(e)=>this.props.handleInputChange(e)} />
                    <label className="NavMenuLabels" htmlFor="password">Hasło</label>
                    <input id="password" className="navMenuListItemInput" name="password"  type="password" placeholder="Hasło" value={this.props.password}  onChange={(e)=>this.props.handleInputChange(e)} />
                    <input type="submit" className="navMenuListItemInput navMenuListButton "  value="Zaloguj się" />
                </form>
               </li>
               <label className="NavMenuLabels" htmlFor="forgetPassword">Zapomnialeś hasła?</label>
              <li className="navMenuListItem"><Link id="forgetPassword" className="navMenuListLink" to="/ResetHasła"><span id="ForgetPasswordTextChange">Zapomnialeś hasła?</span></Link></li>
              <label className="NavMenuLabels" htmlFor="registration">Nie masz jeszcze konta?</label>   
              <li className="navMenuListItem"><Link id="registration" className="navMenuListLink" to="/Rejestracja">Zarejestruj się</Link></li>
            </ul>         
        )
    }
}
