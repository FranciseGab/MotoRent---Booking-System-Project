Frontend/
    src/    
        apis/ - uses the environment for requesting!
            mainApi.ts (main helper uses the nevironment as base api url)
            authApi.ts (uses the mainApi as helper then put the request for auths here!)
        pages/
            login/
                login.html
                login.ts (uses authservice!)
                login.specs.ts
                login.css   (not login.component etc,. since this is modern angular and lets use cli angular to create!)
        components/
            (optional for helper )
        services/
            auth.service.ts(uses the authApi then have business logics here)
        environment/ http://localhost:5185 (the server dotnet api)
