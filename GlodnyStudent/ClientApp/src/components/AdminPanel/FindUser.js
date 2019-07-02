import React, { Component } from 'react'

export default class FindUser extends Component {
    render() {
        const usersList = this.props.users.map(user=><li id={user.username}>{user.username}<button onClick={this.props.banUser} value={user.username}>{user.userStatus}</button></li>);
        return (
            <div>
                <h3>Wyszukaj użytkownika</h3>
                <form id="search" onSubmit={this.props.getUsers} >
                    <input name="nickname" type="text"  placeholder="Podaj nickname użytkownika" onChange={this.props.handleInputChange}/>
                    <input type="submit" value="Szukaj"/>
                </form>
                <ul>
                    {usersList}
                </ul>
            </div>
        )
    }
}
