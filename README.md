# Base.Api

Sample STATELESS API ASP .NET
 
> Amazon WS Authentication logic like

## How it work ?

[Amazon Rest Authentication](http://docs.aws.amazon.com/AmazonS3/latest/dev/RESTAuthentication.html)

This is a sample, the Public Key & Secret Key are in code (maybe a database for the next) !

You are able to configure each request composants in the web.config (request duration, the WS name, the request signature separator, ...).

Each parameters are configurables, take a look to the [web.config](https://github.com/aZerato/Base.Api/blob/master/Base.Api/Web.config).

Key Header | Description
---------- | -----------
webServicesName | It's the first element of "Authorization" header
prefixHeaderKey | The prefix of each custom attributes added to header
valueRequestSeparator | The separator used for the signature creation
requestTimeValidity | The request time validity in ms

## Licence

MIT