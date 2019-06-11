import React, { Component } from 'react';

export default class KRScheck extends Component {

    constructor(props){
        super(props);
        this.state = {
            errorMsg: null,
            KRS:null
        }
        this.CheckKRS = this.CheckKRS.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    CheckKRS(e){
        e.preventDefault();
        fetch(`https://api-v3.mojepanstwo.pl/dane/krs_podmioty.json?conditions[krs_podmioty.krs]=${this.state.KRS}`)
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



    handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
      
        this.setState({
            [name]: value
        });
        
       
    }




    render() {
        return (
            <div>
            <label>Numer KRS <input type="number" disabled={!this.props.wantToBeOwner} name="KRS" onChange={this.handleInputChange}/></label>
            <button onClick={this.CheckKRS} >Sprawdź</button>
            {this.state.errorMsg}
            </div>
    )
}
}