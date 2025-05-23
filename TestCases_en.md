**ID**: 01 \
**Name**: Check browser search title name \
**Precondition**: None \
**Enviroment**: Linux Ubuntu 22.04 Desktop. C# .NET 9.0 SDK \
**Postcondition**: None \
**Steps**:
1. Go to site `https://www.google.com/`
2. Check that there is a title "Google"

**Expecting result**: Title is "Google" \
**Status**: Success

<br>

**ID**: 02 \
**Name**: Check vacancies with applied filters on job site \
**Precondition**: None \
**Enviroment**: Linux Ubuntu 22.04 Desktop. C# .NET 9.0 SDK \
**Postcondition**: None \
**Steps**:
1. Go to site `https://hh.ru/`
2. Input vacancy name ("programmer")
3. Press `Enter`
4. Click on button with filter icon
5. Select `No experience`, `Remote work`
6. Click button "Search"

**Expecting result**: List of vacancies, in filters items `No experience` and `Remote work` are selected \
**Status**: Success