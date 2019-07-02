import React, { Component } from 'react'

export default class ReviewsList extends Component {
    render() {

        const reviewsList = this.props.reviews.map(row=>
            <div key={row.id}>
              <div>
                <ul>
                  <li><i ></i>{row.userUsername }</li> {/* className="far fa-user" */}
                  <li>{row.addTime}</li>
                  <li><button name="CreateReportReview" value={row.id} onClick={(e)=>this.props.makeReport(e)} >Zgłoś komentarz</button></li>
                  <li>{(sessionStorage.getItem("role") === "Admin")?<button  value={row.id} onClick={(e)=>this.props.handleRemoveRevievs(e)}>Usuń komentarz</button>:null}</li>
                </ul>
              </div>
              <div>
                {row.description}
              </div>
            </div>);

        return (
            <div>
                {reviewsList}
              </div> 
        )
    }
}
