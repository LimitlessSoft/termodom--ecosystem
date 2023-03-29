import Cookies from "universal-cookie"
const cookies = new Cookies()

const getItems = () => {
    var cString = cookies.get('cookie-korpa')
    if(cString == null || cString.length == 0) {
        return []
    } else {
        return cString
    }
}

const CookieKorpa = {
    get: function(robaid) {
        var items = getItems()
        if(robaid == null) {
            return items
        } else {
            for(var i = 0; i < items.length; i++) {
                if(items[i].robaid == robaid) {
                    return items[i]
                }
            }
        }
    },
    add: function(robaid, kolicina) {
        var items = getItems()
        var found = -1
        for(var i = 0; i < items.length; i++) {
            if(items[i] != null && items[i].robaid == robaid) {
                found = i
                break
            }
        }
        if(found == -1) {
            items.push({ robaid: robaid, kolicina: kolicina})
        } else {
            items[found].kolicina += kolicina
        }

        cookies.set('cookie-korpa', JSON.stringify(items))
    },
    remove: function(robaid) {
        var items = getItems()
        for(var i = 0; i < items.length; i++) {
            if(items[i].robaid == robaid) {
                items[i] = null
                break
            }
        }
        cookies.set('cookie-korpa', JSON.stringify(items))
    }
  }
export default CookieKorpa