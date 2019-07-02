import React, { Component } from 'react'
import FindUser from './FindUser';
import Reports from './Reports';

export default class AdminPanel extends Component {

    constructor(props){
        super(props);
        this.state={
            nickname:"",
            users:[],
            reports:[],
            openedFind: true,
            openedReports: false,
            notifyDisableBtn:false,
          findUserDisableBtn:true
        }
        this.handleInputChange = this.handleInputChange.bind(this);
        this.getUsers = this.getUsers.bind(this);
        this.banUser = this.banUser.bind(this);
        this.getReports = this.getReports.bind(this);
        this.removeReport = this.removeReport.bind(this);
        this.toggleAdminPanel = this.toggleAdminPanel.bind(this);
        
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

          if(data.status ===200){
            let usersNew = this.state.users     
            let index = usersNew.findIndex(user=> user.username === data.username);
            usersNew[index].userStatus = data.userStatus;
            this.setState({
              users: usersNew
            });
          }
          
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
            if(data.status === 200)
            {  let reportsNew = this.state.reports     
              let index = reportsNew.findIndex(report=> report.id === data.notification.id);
              reportsNew.splice(index, 1);
              this.setState({
                users: reportsNew
              });}
        } )
        .catch((err)=>console.log(err))  
      } 

      toggleAdminPanel(e) {
        const { openedFind,openedReports } = this.state;
        const name = e.target.name;

        this.setState(prevState => ({
          openedFind: !prevState.openedFind,
          openedReports: !prevState.openedReports,
          notifyDisableBtn:name ==="notifyDisableBtn"?true:false,
          findUserDisableBtn:name ==="findUserDisableBtn"?true:false
          
        }));
      }

     

    render() {

    

        const { openedFind, openedReports } = this.state;
        return (
            <div id="adminPanel">
              <div>
                <div>
                  <div>
                    <p>Wybierz opcję, która Cię interesuje</p>
                    <button onClick={this.toggleAdminPanel} disabled={this.state.notifyDisableBtn} name="notifyDisableBtn" id="findButtonAdmin">Wyświetl zgłoszenia</button>
                    <button onClick={this.toggleAdminPanel} disabled={this.state.findUserDisableBtn} name="findUserDisableBtn" id="reportsButtonAdmin">Znajdź użytkownika</button>
                  </div>
                  {openedFind && (
                    <div id="findUserAdmin">
                      <FindUser banUser={this.banUser}  users={this.state.users}  getUsers={this.getUsers} handleInputChange={this.handleInputChange} />
                    </div>
                  )}
                  {openedReports && (
                    <div id="reportsAdmin">
                      <Reports  removeReport={this.removeReport} getReports={this.getReports} reports={this.state.reports} />
                    </div>
                  )}
                </div>
              </div>
            </div>
        )
    }
}
