import React, { Component } from 'react';

export default class KRScheck extends Component {

    constructor(props){
        super(props);
        this.state = {
            errorMsg: null
        }
        this.CheckKRS = this.CheckKRS.bind(this);
    }

    CheckKRS(e){
        e.preventDefault();
        fetch(`https://api-v3.mojepanstwo.pl/dane/krs_podmioty.json?conditions[krs_podmioty.krs]=${this.props.KRS}`)
            .then(res => res.json())
            .then(data =>{ 
                const res =data.Count>0?true:false;
                this.props.setKRSValidationResult(res);
                this.setState({
                    errorMsg:res?"Poprawny numer KRS.":"Niepoprawny numer KRS."
                  });
                }
            )
            .catch((error) => {
                console.log(error);
                this.setState({
                    errorMsg:"Niepoprawny numer KRS."
                  });
              });
    }




    render() {
        return (
            <div>
                 <label>Numer KRS <input type="number" disabled={!this.props.wantToBeOwner} name="KRS" /></label>
                 <button onClick={this.CheckKRS} >Sprawdź</button>
                 {this.state.errorMsg}
            </div>
        )
    }
}
