import React, { Component } from 'react'
import { Link } from 'react-router-dom';

export default class GusetMenu extends Component {

    render() {
        return (
            <div>
                <ul className="navList"> 
                <li><Link to="/ResetHasła">Zapomnialeś hasła?</Link></li>  
                <li><Link to="/Rejestracja">Zarejestruj się</Link></li>
              </ul>
              <form className="formLogin" onSubmit={(e)=>this.props.handleLogIn(e)}>
                  <input name="email" type="text" placeholder="E-mail"  onChange={(e)=>this.props.handleInputChange(e)} />
                  <input name="password"  type="password" placeholder="Hasło" onChange={(e)=>this.props.handleInputChange(e)} /> 
                  <input type="submit" value="Zaloguj się"></input>
              </form>
            </div>
        )
    }
}
