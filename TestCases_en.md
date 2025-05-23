**ID**: 01 \
**Name**: Check browser search title name \
**Precondition**: None \
**Enviroment**: Linux Ubuntu 22.04 Desktop. C# .NET 9.0 SDK \
**Postcondition**: None \
**Steps**:
1. Open browser
2. Go to site `https://www.google.com/`
3. Check that there is a title "Google"
4. Close browser

**Expecting result**: Title is "Google" \
**Status**: Success

<br>

**ID**: 02 \
**Name**: Check vacancies with applied filters on job site \
**Precondition**: None \
**Enviroment**: Linux Ubuntu 22.04 Desktop. C# .NET 9.0 SDK \
**Postcondition**: None \
**Steps**:
1. Open browser
2. Go to site `https://hh.ru/`
3. Input vacancy name ("programmer")
4. Press `Enter`
5. Click on button with filter icon
6. Select `No experience`, `Remote work`
7. Click button "Search"
8. Close browser 

**Expecting result**: List of vacancies with applied filters \
**Status**: Success