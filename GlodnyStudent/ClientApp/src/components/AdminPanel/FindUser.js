import React, { Component } from 'react'

export default class FindUser extends Component {
    render() {
        const usersList = this.props.users.map(user=><li id={user.username}>{user.username}<button onClick={this.props.banUser} value={user.username}>{user.status}</button></li>);
        return (
            <div>
                <h3>Wyszukaj użytkownika</h3>
                <form id="search" className="searchContainer wow fadeInLeft" data-wow-duration="2s" onSubmit={this.props.getUsers} >
                    <input name="nickname" className="searchInput" type="text"  placeholder="Podaj nickname użytkownika" onChange={this.props.handleInputChange}/>
                    <input  className="searchBtn" type="submit" value="Szukaj"/>
                </form>
                <ul>
                    {usersList}
                </ul>
            </div>
        )
    }
}
