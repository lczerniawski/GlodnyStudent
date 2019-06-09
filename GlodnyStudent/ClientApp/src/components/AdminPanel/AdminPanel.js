import React, { Component } from 'react'
import './AdminPanel.css'
import FindUser from './FindUser';
import Reports from './Reports';

export default class AdminPanel extends Component {

    constructor(props){
        super(props);
        this.state={
            nickname:"",
            users:[],
            reports:[]
        }
        this.handleInputChange = this.handleInputChange.bind(this);
        this.getUsers = this.getUsers.bind(this);
        this.banUser = this.banUser.bind(this);
        this.getReports = this.getReports.bind(this);
        this.removeReport = this.removeReport.bind(this);
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
          let usersNew = this.state.users     
          let index = usersNew.findIndex(user=> user.username === data.username);
          usersNew[index].status = data.status;
          this.setState({
            users: usersNew
          });
        } )
        .catch((err)=>console.log(err))  
      } 



      getReports(event) {
         
       if(event)event.preventDefault();
         const adr =`/api/Notifications`;
        
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
              reports:data
          });
        
        } )
        .catch((err)=>console.log(err))  
      } 

      removeReport(event) {
         
        event.preventDefault();
         const adr =` /api/Notifications?id=${event.target.value}`;
        
          fetch(adr, {
          method: 'DELETE',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':'Bearer ' + sessionStorage.getItem("token")
          }
        }).then(res => res.json())
        .then((data) => { 
          let reportsNew = this.state.reports     
          let index = reportsNew.findIndex(report=> report.id === data.id);
          reportsNew.splice(index, 1);
          this.setState({
            users: reportsNew
          });
        } )
        .catch((err)=>console.log(err))  
      } 




    render() {
        
        return (
            <div id="adminPanel">
                <FindUser banUser={this.banUser}  users={this.state.users}  getUsers={this.getUsers} handleInputChange={this.handleInputChange} />
                <Reports  removeReport={this.removeReport} getReports={this.getReports} reports={this.state.reports} />
            </div>
        )
    }
}
