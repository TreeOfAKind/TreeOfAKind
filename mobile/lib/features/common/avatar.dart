import 'package:flutter/material.dart';

class Avatar extends StatelessWidget {
  const Avatar({Key key, this.photo, @required this.avatarSize})
      : super(key: key);

  final String photo;
  final double avatarSize;

  @override
  Widget build(BuildContext context) {
    return CircleAvatar(
      radius: avatarSize,
      backgroundImage: photo != null ? NetworkImage(photo) : null,
      child:
          photo == null ? Icon(Icons.person_outline, size: avatarSize) : null,
    );
  }
}
