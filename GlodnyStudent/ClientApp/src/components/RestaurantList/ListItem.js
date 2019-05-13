import React, { Component } from 'react'
import './ListItem.css';

export default class listItem extends Component {



  PopularityBadge() {
   if(this.props.reviewsCount > 100) return <p className="badge">Popularne Miejsce</p>;
  }


  render() {
    const badge = this.PopularityBadge();
    return (
        <div className="listItemContainer">
          <div className="image"></div> {/* <- w tym divie docelowo bedzie zdjecie  */}
          <div className="restaurantInfo">
            <div>
              <h3>{this.props.name}</h3>
              <address>{this.props.address}</address>
              <p>Ilość opinii: {this.props.reviewsCount}</p>
              {badge}
            </div>
          </div>
        </div>
    )
  }
}
