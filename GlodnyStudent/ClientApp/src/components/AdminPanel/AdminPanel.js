import React, { Component } from 'react'
import './AdminPanel.css'

export default class AdminPanel extends Component {

    constructor(props){
        super(props);
        this.state={
            nickname:"",
            users:[]
        }
        this.handleInputChange = this.handleInputChange.bind(this);
        this.getUsers = this.getUsers.bind(this);
        this.banUser = this.banUser.bind(this);
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });
    
      }


      getUsers(event) {
         
        event.preventDefault();
         const adr =`/api/UsersManager?startsWith=${this.state.nickname}`;
        
          fetch(adr, {
          method: 'GET',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':'Bearer ' + sessionStorage.getItem("token")

          }
        }).then(res => res.json())
        .then((data) => {
          this.setState({
              users:data
          });
        
        } )
        .catch((err)=>console.log(err))  
      } 

      banUser(event) {
         
        event.preventDefault();
         const adr =`/api/UsersManager`;
        
          fetch(adr, {
          method: 'POST',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':'Bearer ' + sessionStorage.getItem("token")
          },
          body: JSON.stringify(
            event.target.value
            )
        }).then(res => res.json())
        .then((data) => {
          
        
        } )
        .catch((err)=>console.log(err))  
      } 



    render() {
        const usersList = this.state.users.map(user=><li id={user}>{user}<button onClick={this.banUser} value={user}>X</button></li>);
        return (
            <div id="adminPanel">
                <h3>Wyszukaj użytkownika</h3>
                <form id="search" className="searchContainer wow fadeInLeft" data-wow-duration="2s" onSubmit={this.getUsers} >
                    <input name="nickname" className="searchInput" type="text"  placeholder="Podaj nickname użytkownika" onChange={this.handleInputChange}/>
                    <input  className="searchBtn" type="submit" value="Szukaj"/>
                </form>
                <ul>
                    {usersList}
                </ul>
            </div>
        )
    }
}
