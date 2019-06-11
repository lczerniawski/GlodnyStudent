import React, { Component } from 'react'

export default class ReviewsList extends Component {
    render() {

        const reviewsList = this.props.reviews.map(row=>
            <div className="singleReview wow fadeIn" data-wow-duration="1s" key={row.id}>
              <div className="details">
                <ul>
                  <li><i className="far fa-user"></i>{row.userUsername }</li>
                  <li>{row.addTime}</li>
                  <li><button className="reportComment" name="CreateReportReview" value={row.id} onClick={(e)=>this.props.makeReport(e)} >Zgłoś komentarz</button></li>
                </ul>
              </div>
              <div className="description">
                {row.description}
              </div>
            </div>);

        return (
            <div className="reviewsList">
                {reviewsList}
              </div> 
        )
    }
}
