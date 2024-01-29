import React, { useState, useEffect } from "react";
import debug from "sabio-debug";
import lookUpService from "../../services/lookUpService";
import { mapLookUpItem } from "../../helper/utils";

const _logger = debug.extend("LinkListLookUp");

function LinkListLookUp() {
  const [lookUps, setLookUps] = useState({
    urlTypes: [],
    mappedUrlTypes: [],
  });

  useEffect(() => {
    lookUpService.lookUp(["UrlTypes"]).then(onUrlTypesSuccess).catch(onError);
  }, []);

  const onError = (error) => {
    _logger("Error getting types", error);
  };

  const onUrlTypesSuccess = (data) => {
    _logger("DATA", data);
    const urlTypes = data.item.urlTypes;
    setLookUps((prevState) => {
      let newState = { ...prevState };
      newState.urlTypes = urlTypes;
      newState.mappedUrlTypes = urlTypes.map(mapLookUpItem);
      return newState;
    });
  };

  return <>{lookUps.mappedUrlTypes}</>;
}

export default LinkListLookUp;
