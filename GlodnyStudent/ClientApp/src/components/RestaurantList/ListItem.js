import React, { Component } from 'react'
import './ListItem.css';
import PropTypes from 'prop-types';

export default class listItem extends Component {


  constructor(props){
    super(props);
    this.handleClickElement= this.handleClickElement.bind(this);
  }


  PopularityBadge() {
   if(this.props.reviewsCount > 100) return <p className="badge">Popularne Miejsce</p>;
  }


  handleClickElement(event) {
    this.props.sendId(this.props.id);
     this.context.router.history.push(`/RestaurantPage`);
     event.preventDefault();
   }


  static contextTypes = {
    router: PropTypes.object
  }


  render() {
    const badge = this.PopularityBadge();
    return (
        <div className="listItemContainer" onClick={this.handleClickElement}>
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
