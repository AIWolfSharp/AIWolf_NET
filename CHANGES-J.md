* 1.0.0: �ŏ��̃����[�X
* 1.1.0: �Q�[���T�[�o�������.NET�ł����Ŋ�������悤�ɂȂ����̂ɔ����C
���̂��u���C�u�����v����u�v���b�g�t�H�[���v�ɕς��܂����D
  * �Q�[���T�[�o�������܂����D
�������C���b������`�F�b�N�Ǝ��Ԑ����͎������Ă��܂���D
  * �v�Z�R�X�g�팸�̂��߁C�G�[�W�F���g-�T�[�o�Ԃł̃N���X�̋��p�̕��@���������܂����D
  * �Ӑ}���Ȃ����������ɂ��듮���h�~���邽�߁C
�G�[�W�F���g�ɓn�����GameInfo, GameSetting�����������s�ɂ��܂����D
  * �e��N���C�A���g�X�^�[�^���v���Z�X�Ƃ��ċN������GameStarter�������܂����D
�T�[�o�N����CJava, .NET, Python�Ȃǂ̃G�[�W�F���g��ڑ����đΐ킷�邱�Ƃ��ł��܂��D
* 2.0.0: API�̕ύX
  * �V�K�N���X
    * AIWolf.Lib.AbstractRoleAssignPlayer
  * �V�K�C���^�[�t�F�[�X
    * AIWolf.Lib.IGameInfo
    * AIWolf.Lib.IGameSetting
    * AIWolf.Lib.IUtterance
  * AIWolf.Lib.Content�N���X
    * �v���p�e�B�̌^�ύX
      * `public IUtterance Utterance { get; }`
      * `public IList<Content> ContentList { get; }`
    * �R���X�g���N�^�̕s����
      * `internal Content(Content content)`
  * AIWolf.Lib.ContentBuilder�N���X�̒��ۉ�
  * AIWolf.Lib.IPlayer�C���^�[�t�F�[�X
    * ���\�b�h�����̌^�ύX
      * `void Update(IGameInfo gameinfo)`
      * `void Initialize(IGameInfo gameInfo, IGameSetting gameSetting)`
  * AIWolf.Lib.ShuffleExtensions.Shuffle�g�����\�b�h
    * �߂�l�̌^�ύX
      * `public static IList<T> Shuffle<T>(this IEnumerable<T> s)`