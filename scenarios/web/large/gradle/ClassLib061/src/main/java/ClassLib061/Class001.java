package ClassLib061;

public class Class001 {
    public static String property() {
        return "ClassLib061" + " " + ClassLib051.Class001.property() + " " + ClassLib015.Class001.property();
    }
}
