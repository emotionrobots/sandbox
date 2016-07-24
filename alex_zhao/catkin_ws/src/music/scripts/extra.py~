79430918


client = soundcloud.Client(client_id='51f459c0a6b2ee82ce31d9a691e2d6fb',
    client_secret = '08323d9fd262b0e667d35c68ff136848',
    username='azhao6@gmail.com',
    password = 'ecpl00')

    token = client.access_token
    print token

    other_client = soundcloud.Client(access_token = token)
    print other_client.get('/me').username

    print other_client.get('/me').description
    
    client_two = soundcloud.Client(client_id = '51f459c0a6b2ee82ce31d9a691e2d6fb')
#    track_url = 'https://soundcloud.com/alex-zhao-993238266/sets/nora'
#    embed_info = client_two.get('/oembed', url=track_url)
#    print embed_info

    track = client.get('/tracks/258538227')
    stream_url = client.get(track.stream_url, allow_redirects=False)
    print stream_url.location

    track_id = '79430918'
    client_id = '51f459c0a6b2ee82ce31d9a691e2d6fb'
    url = 'http://api.soundcloud.com/tracks/79430918/stream?client_id=51f459c0a6b2ee82ce31d9a691e2d6fb'

    json_obj = urllib2.urlopen(url)
    data = json.load(json_obj)
